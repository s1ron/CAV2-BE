using ChatAppBackEndV1.Data.Context;
using ChatAppBackEndV1.Data.Entities;
using ChatAppBackEndV1.Services.FriendService;
using ChatAppBackEndV2.Data.Enums;
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
                                  FirstName = z.user.FirstName,
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
                                        LastMessage = _context.Messages.Where(x=>x.ConversationId == con.Id).OrderByDescending(x=>x.SendAt).First(),
                                        CreateAt = con.CreateAt,
                                        IsFavoriteConversation = par.IsFavoriteConversation,
                                        AuthorId = con.AuthorId,
                                        ParticipantUser = parUser[con.Id].Object.ToList(),
                                    };
            var orderby = await conversationQuery.OrderBy(x=>x.LastMessage.SendAt).ThenBy(x=>x.CreateAt).ToListAsync();

            return orderby;
        }

        public async Task<CollapseConversationResponse> GetCollapseConversationByConversationIdAsync(Guid userId, long conversationId)
        {
            var conversationQuery = await _context.Conversations.FindAsync(conversationId);

            var parti = await (from user in _context.Users
                    join par in _context.Participants on user.Id equals par.UserId
                    where par.ConversationId == conversationId
                    select new ParticipantUserResponse()
                    {
                        LastReadMessageId = par.LastReadMessageId,
                        NickName = par.NickName,
                        UserId = par.UserId,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        ProfileImagePath = user.ProfileImagePath,
                        ProfileDescription = user.ProfileDescription,
                        UserName = user.UserName,
                        Gender = user.Gender,
                        IsFavoriteConversation = par.IsFavoriteConversation
                    }).ToListAsync();


            var result = new CollapseConversationResponse()
            {
                IsGroup = conversationQuery.IsGroup,
                ConversationId = conversationId,
                ConversationName = conversationQuery.ConversationName,
                IsMessageRequest = conversationQuery.IsMessageRequest,
                ConversationImagePath = conversationQuery.ConversationImagePath,
                IsBlock = conversationQuery.IsBlock,
                BlockBy = conversationQuery.IsBlock ? conversationQuery.BlockBy : null,
                ConversationThemeId = conversationQuery.ConversationThemeId,
                QuickMessage = conversationQuery.QuickMessage,
                LastMessage = _context.Messages.Where(x => x.ConversationId == conversationId).OrderByDescending(x => x.SendAt).FirstOrDefault(),
                CreateAt = conversationQuery.CreateAt,
                IsFavoriteConversation = parti.FirstOrDefault(x => x.UserId == userId).IsFavoriteConversation,
                AuthorId = conversationQuery.AuthorId,
                ParticipantUser = parti
            };

            return result;

        }

        public async Task<long> GetOrCreateConversation(Guid userId, Guid friendId)
        {
            var getconversationId = await GetConversationId(userId, friendId);
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
                QuickMessage = "system/quick/default.png",
                ConversationThemeId = 1
            };
            await _context.Conversations.AddAsync(conversation);
            await _context.SaveChangesAsync();
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
            await _context.SaveChangesAsync();
            return conversation.Id;
        }
        private async Task<long> GetConversationId(Guid userId, Guid friendId)
        {
            var conversation = await (from con in _context.Conversations
                               join par in _context.Participants on con.Id equals par.ConversationId
                               where con.IsGroup == false && (par.UserId == userId || par.UserId == friendId)
                               select par.ConversationId).GroupBy(x=>x).ToListAsync();
            foreach(var gr in conversation)
            {
                if (gr.Count() == 2)
                {
                    return gr.Key;
                }
            }
            return 0;
        }

        public async Task<List<ConversationTheme>> GetListConversationTheme()
        {
            return await _context.ConversationThemes.ToListAsync();
        }

        public async Task<List<Attachment>> GetConversationAttachment(long conversationId)
        {
            //var a = await _context.Attachments.Where(x => x.FilePath.StartsWith($"attachment\\attachment-{conversationId}")).ToListAsync();
            var sa = await (from at in _context.Attachments
                            join m in _context.Messages on at.MessageId equals m.Id
                            where at.FilePath.StartsWith($"attachment\\attachment-{conversationId}") && (m.MessageType == MessageTypeEnum.IMAGE || m.MessageType == MessageTypeEnum.VIDEO)
                            select at).ToListAsync();
            return sa;
        }
    }
}
