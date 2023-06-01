using ChatAppBackEndV1.Data.Context;
using ChatAppBackEndV2.Dtos.MessageService;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;

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
            return messagequery.AsEnumerable().SkipWhile(num=> num.m.Id <= lastMessageId).Take(10).Select(x =>
            {
                var reaction = from mr in _context.MessagesReactions
                               join u in _context.Users on mr.UserId equals u.Id into mru
                               from u in mru.DefaultIfEmpty()
                               where mr.MessageId == x.m.Id
                               select new { mr, u };
                var reacttionlist = reaction.Select(q => new ReactionOfAMessage()
                {
                    UserId = q.u.Id,
                    FirstName = q.u.FirstName,
                    LastName = q.u.LastName,
                    UserName = q.u.UserName,
                    ProfileImagePath = q.u.ProfileImagePath,
                    ReactionType = q.mr.ReactionType,
                    MessageId = x.m.Id
                }).ToList();
                return new SingleMessageResponse()
                {
                    Id = x.m.Id,
                    ConversationId = conversationId,
                    SenderId = x.m.SenderId,
                    Content = x.m.Content,
                    MessageType = x.m.MessageType,
                    SendAt = x.m.SendAt,
                    FilePath = x.at.FilePath,
                    FileSize = x.at.FileSize,
                    MessageReaction = reacttionlist
                };
            }).ToList();
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
            var message = await messagequery.Take(10).Select(s => new SingleMessageResponse()
            {
                Id = s.m.Id,
                ConversationId = conversationId,
                SenderId = s.m.SenderId,
                Content = s.m.Content,
                MessageType = s.m.MessageType,
                SendAt = s.m.SendAt,
                FilePath = s.at.FilePath,
                FileSize = s.at.FileSize,
                MessageReaction = yed==null ? null : yed.ContainsKey(s.m.Id) ? yed[s.m.Id].Object.ToList() : null,
                //MessageReaction = yed.GetValueOrDefault(s.m.Id).Object.ToList()
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





            /*
             * 
             * 
             * yed.GetValueOrDefault(s.m.Id).Object.ToList()
                        var messagequery = from m in _context.Messages
                                           join at in _context.Attachments on m.Id equals at.MessageId into mat
                                           from at in mat.DefaultIfEmpty()
                                           where m.ConversationId == conversationId
                                           orderby m.SendAt descending
                                           select new { m, at };
                        var par = await (from u in _context.Users
                                   join p in _context.Participants on u.Id equals p.UserId
                                   where p.ConversationId == conversationId
                                   select new { u, p }).Select(x => new ParticipantUserResponse()
                                   {
                                       LastReadMessageId = x.p.LastReadMessageId,
                                       NickName = x.p.NickName,
                                       UserId = x.u.Id,
                                       UserName = x.u.UserName,
                                       FullName= x.u.FullName,
                                       Email = x.u.Email,
                                       ProfileDescription = x.u.ProfileDescription,
                                       ProfileImagePath = x.u.ProfileImagePath
                                   }).ToListAsync();
                        var message = messagequery.AsEnumerable().Take(10).Select(x =>
                        {
                            var reaction = from mr in _context.MessagesReactions
                                            join u in _context.Users on mr.UserId equals u.Id into mru
                                            from u in mru.DefaultIfEmpty()
                                            where mr.MessageId == x.m.Id
                                            select new { mr, u };
                            var reacttionlist = reaction.Select(q => new ReactionOfAMessage()
                            {
                                UserId = q.u.Id,
                                FullName = q.u.FullName,
                                UserName = q.u.UserName,
                                ProfileImagePath = q.u.ProfileImagePath,
                                ReactionType = q.mr.ReactionType,
                                MessageId = x.m.Id
                            }).ToList();
                            return new SingleMessageResponse()
                            {
                                Id = x.m.Id,
                                ConversationId = conversationId,
                                SenderId = x.m.SenderId,
                                Content = x.m.Content,
                                MessageType = x.m.MessageType,
                                SendAt = x.m.SendAt,
                                FilePath = x.at.FilePath,
                                FileSize = x.at.FileSize,
                                MessageReaction = reacttionlist
                            };
                        }).ToList();

                        return new FirstMessageResponse()
                        {
                            IsGroup = conversation.IsGroup,
                            ConversationId = conversationId,
                            ConversationName = conversation.ConversationName,
                            AuthorId= conversation.AuthorId,
                            IsMessageRequest= conversation.IsMessageRequest,
                            ConversationImagePath= conversation.ConversationImagePath,
                            IsBlock = conversation.IsBlock,
                            BlockBy = conversation.BlockBy,
                            ConversationThemeId= conversation.ConversationThemeId,
                            QuickMessage = conversation.QuickMessage,

                            ConversationTheme = conversationTheme,
                            Participants = par,
                            Messages = message
                        };*/
        }
    }
}
