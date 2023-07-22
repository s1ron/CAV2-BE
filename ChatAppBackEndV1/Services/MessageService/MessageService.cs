using ChatAppBackEndV1.Data.Context;
using ChatAppBackEndV2.Dtos.MessageService;
using Microsoft.EntityFrameworkCore;
using ChatAppBackEndV2.Data.Enums;

namespace ChatAppBackEndV2.Services.MessageService
{
    public class MessageService : IMessageService
    {
        private readonly ChatAppDbContext _context;
        public MessageService(ChatAppDbContext context)
        {
            _context = context;
        }
        class Test{
            public long MessageId { get; set; }
            public IEnumerable<ReactionOfAMessage> Object { get; set; }
        }

        public async Task<List<SingleMessageResponse>> GetContinueMessage(long conversationId, long lastMessageId)
        {
            var messagequery = from m in _context.Messages
                               join at in _context.Attachments on m.Id equals at.MessageId into mat
                               from at in mat.DefaultIfEmpty()
                               where m.ConversationId == conversationId
                               orderby m.SendAt descending
                               select new { m, at };
            Dictionary<long, Test> yed = null;
            if (messagequery != null)
            {
                yed = await (from mq in messagequery
                             join rea in _context.MessagesReactions on mq.m.Id equals rea.MessageId
                             join u in _context.Users on rea.UserId equals u.Id
                             select new { mq, rea, u }).GroupBy(g => g.rea.MessageId).Select(x =>
                             new Test()
                             {
                                 MessageId = x.Key,
                                 Object = x.Select(z => new ReactionOfAMessage()
                                 {
                                     UserId = z.u.Id,
                                     FirstName = z.u.FirstName,
                                     LastName = z.u.LastName,
                                     UserName = z.u.UserName,
                                     ProfileImagePath = z.u.ProfileImagePath,
                                     ReactionType = z.rea.ReactionType,
                                     MessageId = z.rea.MessageId
                                 })
                             }).ToDictionaryAsync(q => q.MessageId);
            }
            var message = messagequery.AsEnumerable().SkipWhile(z=>z.m.Id >= lastMessageId).Take(15).Select(s => new SingleMessageResponse()
            {
                Id = s.m.Id,
                ConversationId = conversationId,
                SenderId = s.m.SenderId,
                Content = s.m.Content,
                MessageType = s.m.MessageType,
                SendAt = s.m.SendAt,
                FilePath = (s.m.MessageType == MessageTypeEnum.QUICK || s.m.MessageType == MessageTypeEnum.GIF || s.m.MessageType == MessageTypeEnum.STICKER) ? s.m.NonFilePath : s?.at?.FilePath,
                FileSize = s?.at?.FileSize,
                MessageReaction = yed == null ? null : yed.ContainsKey(s.m.Id) ? yed[s.m.Id].Object.ToList() : null,
            }).ToList();
            return message;
        }

        public async Task<FirstMessageResponse> GetFirstMessageAsync(long conversationId)
        {
            var conversation = await _context.Conversations.FindAsync(conversationId);
            var conversationTheme = await _context.ConversationThemes.FindAsync(conversation.ConversationThemeId);
            var messagequery = from m in _context.Messages
                               join at in _context.Attachments on m.Id equals at.MessageId into mat
                               from at in mat.DefaultIfEmpty()
                               where m.ConversationId == conversationId
                               orderby m.SendAt descending
                               select new { m, at };
            Dictionary<long, Test> yed = null;
            if (messagequery != null)
            {
                yed = await (from mq in messagequery
                                join rea in _context.MessagesReactions on mq.m.Id equals rea.MessageId
                                join u in _context.Users on rea.UserId equals u.Id
                                select new { mq, rea, u }).GroupBy(g => g.rea.MessageId).Select(x =>
                                new Test()
                                {
                                    MessageId = x.Key,
                                    Object = x.Select(z => new ReactionOfAMessage()
                                    {
                                        UserId = z.u.Id,
                                        FirstName = z.u.FirstName,
                                        LastName = z.u.LastName,
                                        UserName = z.u.UserName,
                                        ProfileImagePath = z.u.ProfileImagePath,
                                        ReactionType = z.rea.ReactionType,
                                        MessageId = z.rea.MessageId
                                    })
                                }).ToDictionaryAsync(q=>q.MessageId);
            }
            var message = await messagequery.Take(15).Select(s => new SingleMessageResponse()
            {
                Id = s.m.Id,
                ConversationId = conversationId,
                SenderId = s.m.SenderId,
                Content = s.m.Content,
                MessageType = s.m.MessageType,
                SendAt = s.m.SendAt,
                FilePath = (s.m.MessageType == MessageTypeEnum.QUICK || s.m.MessageType == MessageTypeEnum.GIF || s.m.MessageType == MessageTypeEnum.STICKER) ? s.m.NonFilePath : s.at.FilePath,
                FileSize = s.at.FileSize,
                MessageReaction = yed==null ? null : yed.ContainsKey(s.m.Id) ? yed[s.m.Id].Object.ToList() : null,
            }).ToListAsync();
            var par = await (from u in _context.Users
                             join p in _context.Participants on u.Id equals p.UserId
                             where p.ConversationId == conversationId
                             select new { u, p }).Select(x => new ParticipantUserResponse()
                             {
                                 LastReadMessageId = x.p.LastReadMessageId,
                                 NickName = x.p.NickName,
                                 UserId = x.u.Id,
                                 UserName = x.u.UserName,
                                 FirstName = x.u.FirstName,
                                 LastName = x.u.LastName,
                                 Email = x.u.Email,
                                 ProfileDescription = x.u.ProfileDescription,
                                 ProfileImagePath = x.u.ProfileImagePath
                             }).ToListAsync();
            return new FirstMessageResponse()
            {
                IsGroup = conversation.IsGroup,
                CreateAt = conversation.CreateAt,
                ConversationId = conversationId,
                ConversationName = conversation.ConversationName,
                AuthorId = conversation.AuthorId,
                IsMessageRequest = conversation.IsMessageRequest,
                ConversationImagePath = conversation.ConversationImagePath,
                IsBlock = conversation.IsBlock,
                BlockBy = conversation.BlockBy,
                ConversationThemeId = conversation.ConversationThemeId,
                QuickMessage = conversation.QuickMessage,

                ConversationTheme = conversationTheme,
                ParticipantUser = par,
                Messages = message
            };
        }
    }
}
