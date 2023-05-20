using ChatAppBackEndV1.Data.Context;
using ChatAppBackEndV1.Data.Entities;
using ChatAppBackEndV1.Services.FriendService;
using ChatAppBackEndV2.Dtos.ConversationService;
using ChatAppBackEndV2.Dtos.MessageService;
using HotChocolate.Execution.Processing;
using Microsoft.EntityFrameworkCore;

namespace ChatAppBackEndV2.Services.ConversationService
{
    public class ConversationService : IConversationService
    {
        private readonly ChatAppDbContext _context;
        private readonly IFriendService _friendService;

        public ConversationService(ChatAppDbContext context,
            IFriendService friendService)
        {
            _context = context;
            _friendService = friendService;
        }
        public async Task<List<CollapseConversationResponse>> GetCollapseConversationsAsync(Guid userId)
        {
            var parUser = (from user in _context.Users
                          join par in _context.Participants on user.Id equals par.UserId
                          select new {par, user}).GroupBy(g => g.par.ConversationId).Select(s =>
                          new 
                          {
                              ConversationId = s.Key,
                              Object = s.Select(z => new ParticipantUserResponse()
                              {
                                  LastReadMessageId = z.par.LastReadMessageId,
                                  NickName = z.par.NickName,
                                  UserId= z.par.UserId,
                                  FirstName = z.user.FisrtName,
                                  LastName = z.user.LastName,
                                  Email = z.user.Email,
                                  ProfileImagePath = z.user.ProfileImagePath,
                                  ProfileDescription = z.user.ProfileDescription,
                                  UserName = z.user.UserName,
                                  Gender = z.user.Gender,
                              })
                            }
                        ).ToDictionary(x => x.ConversationId);
            var conversationQuery = from user in _context.Users
                                    join par in _context.Participants on user.Id equals par.UserId
                                    join con in _context.Conversations on par.ConversationId equals con.Id
                                    where user.Id == userId
                                    select new CollapseConversationResponse()
                                    {
                                        IsGroup = con.IsGroup,
                                        ConversationId = con.Id,
                                        ConversationName = con.ConversationName,
                                        IsMessageRequest = con.IsMessageRequest,
                                        ConversationImagePath = con.ConversationImagePath,
                                        IsBlock = con.IsBlock,
                                        BlockBy = con.IsBlock ? con.BlockBy : null,
                                        ConversationThemeId = con.ConversationThemeId,
                                        QuickMessage = con.QuickMessage,
                                        //NickName = par.NickName,
                                        LastMessage = _context.Messages.Where(x=>x.ConversationId == con.Id).OrderByDescending(x=>x.SendAt).First(),
                                        CreateAt = con.CreateAt,
                                        IsFavoriteConversation = par.IsFavoriteConversation,
                                        AuthorId = con.AuthorId,
                                        ParticipantUser = parUser[con.Id].Object.ToList(),
                                    };
            //conversationQuery.OrderByDescending(x => { x.LastMessage == null ? x.CreateAt : x.LastMessage.SendAt; });

            var orderby = conversationQuery.OrderByDescending(x => x.LastMessage.SendAt)
                .ThenBy(z => z.LastMessage.SendAt == null ? z.CreateAt : DateTime.MaxValue);


            return await orderby.ToListAsync();

            /*var conversation = conversationQuery.ToList().Select(x => 
             * 
             * _context.Participants.GroupBy(x => x.ConversationId).ToDictionary(k => k.Key)
            {
                AppUser getuser = null;
                if (!x.con.IsGroup)
                {
                    var otheruser = from u in _context.Users
                                    join p in _context.Participants on u.Id equals p.UserId
                                    where p.ConversationId == x.con.Id && u.Id != userId
                                    select u;
                    getuser = _context.Users.Find(otheruser.FirstOrDefault().Id);
                }
                var lastmess = _context.Messages
                            .Where(a => a.ConversationId == x.con.Id)
                            .OrderByDescending(b => b.SendAt).First();
                return new CollapseConversationResponse()
                {
                    IsGroup = x.con.IsGroup,
                    ConversationId = x.con.Id,
                    ConversationName = x.con.IsGroup ? x.con.ConversationName : getuser.FullName,
                    IsMessageRequest= x.con.IsMessageRequest,
                    ConversationImagePath = x.con.IsGroup ? x.con.ConversationImagePath : getuser.ProfileImagePath,
                    IsBlock = x.con.IsBlock,
                    BlockBy = x.con.IsBlock ? x.con.BlockBy : null,
                    ConversationThemeId = x.con.ConversationThemeId,
                    QuickMessage = x.con.QuickMessage,
                    LastMessage = lastmess.Content,
                    LastMessageDate = lastmess.SendAt,
                    IsFavoriteConversation = x.par.IsFavoriteConversation,
                    AuthorId = x.con.AuthorId
                };
            }).ToList();
            return conversation;*/
        }

        public async Task<long> GetOrCreateConversation(Guid userId, Guid friendId)
        {
            var getconversationId = GetConversationId(userId, friendId);
            if(getconversationId != 0)
            {
                return getconversationId;
            }

            var status = await _friendService.CheckFriendStatusAsync(userId, friendId);

            var conversation = new Conversation()
            {
                CreateAt = DateTime.Now,
                IsGroup = false,
                IsMessageRequest = !status.IsFriend,
                IsBlock = false,
                QuickMessage = "",
                ConversationThemeId = 1
            };
            await _context.Conversations.AddAsync(conversation);
            var list = new List<Participant>()
            {
                new Participant()
                {
                    ConversationId = conversation.Id,
                    UserId= userId,
                    IsFavoriteConversation = false
                },
                new Participant()
                {
                    ConversationId = conversation.Id,
                    UserId= friendId,
                    IsFavoriteConversation = false
                },
            };
            await _context.Participants.AddRangeAsync(list);
            return conversation.Id;
        }
        private long GetConversationId(Guid userId, Guid friendId)
        {
            var conversation = from con in _context.Conversations
                               join par in _context.Participants on con.Id equals par.ConversationId
                               where con.IsGroup == false && (par.UserId == userId || par.UserId == userId)
                               group con by con.Id into congr
                               select congr;
            foreach(var gr in conversation)
            {
                if (gr.Count() == 2)
                {
                    return gr.Key;
                }
            }
            return 0;
        }
    }
}
