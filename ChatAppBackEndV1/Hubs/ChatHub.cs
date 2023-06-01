using ChatAppBackEndV1.Data.Context;
using ChatAppBackEndV1.Data.Entities;
using ChatAppBackEndV1.Services.FriendService;
using ChatAppBackEndV2.Data.Enums;
using ChatAppBackEndV2.Dtos.ChatHubDtos;
using ChatAppBackEndV2.Dtos.FriendService;
using ChatAppBackEndV2.Dtos.MessageService;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace ChatAppBackEndV2.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatAppDbContext _context;
        private readonly IDictionary<Guid, string> _connectedUsers;

        public ChatHub(ChatAppDbContext context,
            IDictionary<Guid, string> connectedUsers)
        {
            _context= context;
            _connectedUsers= connectedUsers;
        }

        public bool CheckUserConnected(Guid userId)
        {
            try
            {
                var a = _connectedUsers[userId];
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }

        public List<FriendResponse> GetOnlineUsers(List<FriendResponse> friendOfUser)
        {
            var b = (from fr in friendOfUser
                     join onu in _connectedUsers on fr.FriendId equals onu.Key
                     select fr).ToList();
            return b;
        }

        public override Task OnConnectedAsync()
        {
            var cid = Context.ConnectionId;
            var uid = Context.GetHttpContext().Request.Query["userid"].ToString();
            var id = new Guid(uid);
            _connectedUsers.Add(id, cid);

            return base.OnConnectedAsync();

        }

        public async Task ConnectedEvent(Guid userId)
        {
            var friendUser = await (from fr in _context.Friends
                              join user in _context.Users on fr.FriendId equals user.Id
                              where fr.UserId == userId
                              select fr.FriendId).ToListAsync();
            var friendOnline = (from fu in friendUser
                                join onu in _connectedUsers on fu equals onu.Key
                                select onu.Value).ToList();

            if (friendOnline.Count > 0)
            {
                var user = _context.Users.Find(userId);
                var userRes = new FriendResponse()
                {
                    FriendId = userId,
                    AddAt = DateTime.Now,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Dob = user.Dob,
                    Gender = user.Gender,
                    ProfileDescription = user.ProfileDescription,
                    ProfileImagePath = user.ProfileImagePath,
                };
                foreach (var conid in friendOnline)
                {
                    await Clients.Client(conid).SendAsync("ReceiverOnlineFriend", userRes);
                }

            }
        }
        
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var a = _connectedUsers.FirstOrDefault(x=>x.Value == Context.ConnectionId);
            _connectedUsers.Remove(a.Key);

            var friendUser = (from fr in _context.Friends
                                   join user in _context.Users on fr.FriendId equals user.Id
                                   where fr.UserId == a.Key
                                   select fr.FriendId).ToList();
            var friendOnline = (from fu in friendUser
                                join onu in _connectedUsers on fu equals onu.Key
                                select onu).ToList();

            foreach (var conid in friendOnline)
            {
                Clients.Client(conid.Value).SendAsync("ReceiverOfflineFriend", a.Key);
            }

            return base.OnDisconnectedAsync(exception);
        }


        public async Task<object> SendMessage(SendMessageResquest sendMessageResquest)
        {
            var message = new Message()
            {
                ConversationId = sendMessageResquest.ConversationId,
                SenderId = sendMessageResquest.SenderId,
                Content = sendMessageResquest.Content,
                MessageType = (MessageTypeEnum)Enum.Parse(typeof(MessageTypeEnum), sendMessageResquest.MessageType),
                SendAt = sendMessageResquest.SendAt.ToLocalTime(),
            };

            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();

            if (message.MessageType == MessageTypeEnum.IMAGE ||
                message.MessageType == MessageTypeEnum.FILE ||
                message.MessageType == MessageTypeEnum.VIDEO ||
                message.MessageType == MessageTypeEnum.AUDIO
                )
            {
                var att = new Attachment()
                {
                    MessageId = message.Id,
                    FilePath = sendMessageResquest.FilePath,
                    FileSize = (long)sendMessageResquest.FileSize
                };
                await _context.Attachments.AddAsync(att);
                await _context.SaveChangesAsync();
            }

            var a = await (from con in _context.Conversations
                    join par in _context.Participants on con.Id equals par.ConversationId
                    where par.UserId != message.SenderId && con.Id == message.ConversationId
                    select par.UserId).ToListAsync();

            var b = (from uid in a 
                          join onu in _connectedUsers on uid equals onu.Key
                          select onu.Value
                          ).ToList();
            if (b.Count != 0)
            {
                var messageRes = new MessageResponseFromHub()
                {
                    Id = message.Id,
                    ConversationId = message.ConversationId,
                    SenderId = message.SenderId.ToString(),
                    Content = message.Content,
                    MessageType = message.MessageType.ToString(),
                    SendAt = message.SendAt,
                    FilePath = sendMessageResquest.FilePath,
                    FileSize = sendMessageResquest.FileSize
                };
                foreach (var conid in b)
                {
                    Clients.Client(conid).SendAsync("ReceiverMessage", messageRes);
                }
            }
            return new { message.Id, message.SendAt };
        }

        public async Task ChangeEmoji(ChangeEmojiRequest changeEmojiRequest)
        {
            var par = await _context.Participants.Where(x => x.ConversationId == changeEmojiRequest.ConversationId && x.UserId != changeEmojiRequest.SenderId).ToListAsync();
            var onlPar = (from p in par 
                         join o in _connectedUsers on p.UserId equals o.Key
                         select o.Value).ToList();

            var user = await _context.Users.FindAsync(changeEmojiRequest.SenderId);
            var conversation = await _context.Conversations.FindAsync(changeEmojiRequest.ConversationId);

            var message = new Message()
            {
                ConversationId = changeEmojiRequest.ConversationId,
                SenderId = changeEmojiRequest.SenderId,
                Content = $"{user.FirstName} has been change emoji",
                MessageType = MessageTypeEnum.SYSTEM,
                SendAt = DateTime.Now,
            };
            conversation.QuickMessage = changeEmojiRequest.NewEmoji;
            //await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();
            var messageRes = new MessageResponseFromHub()
            {
                Id = message.Id,
                ConversationId = message.ConversationId,
                SenderId = message.SenderId.ToString(),
                Content = message.Content,
                MessageType = message.MessageType.ToString(),
                SendAt = message.SendAt,
                FilePath = null,
                FileSize = null
            };
            foreach (var con in onlPar)
            {
                Clients.Client(con).SendAsync("ReceiverNewEmoji", changeEmojiRequest);
                Clients.Client(con).SendAsync("ReceiverMessage", messageRes);
            }
            Clients.Caller.SendAsync("ReceiverMessage", messageRes);
        }
        
        public async Task ChangeTheme(ChangeThemeRequest changeThemeRequest)
        {
            var par = await _context.Participants.Where(x => x.ConversationId == changeThemeRequest.ConversationId && x.UserId != changeThemeRequest.SenderId).ToListAsync();
            var onlPar = (from p in par
                          join o in _connectedUsers on p.UserId equals o.Key
                          select o.Value).ToList();
            var newtheme = await _context.ConversationThemes.FindAsync(changeThemeRequest.NewThemeId);
            var user = await _context.Users.FindAsync(changeThemeRequest.SenderId);
            var conversation = await _context.Conversations.FindAsync(changeThemeRequest.ConversationId);
            
            var message = new Message()
            {
                ConversationId = changeThemeRequest.ConversationId,
                SenderId = changeThemeRequest.SenderId,
                Content = $"{user.FirstName} has been change theme to {newtheme.ThemeName}",
                MessageType = MessageTypeEnum.SYSTEM,
                SendAt = DateTime.Now,
            };

            conversation.ConversationThemeId = changeThemeRequest.NewThemeId;

            //await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();

            var messageRes = new MessageResponseFromHub()
            {
                Id = message.Id,
                ConversationId = message.ConversationId,
                SenderId = message.SenderId.ToString(),
                Content = message.Content,
                MessageType = message.MessageType.ToString(),
                SendAt = message.SendAt,
                FilePath = null,
                FileSize = null
            };

            var themeres = new ChangeThemeResponse()
            {
                Id = newtheme.Id,
                BgColor = newtheme.BgColor,
                BgType = newtheme.BgColor,
                OwnMessageColor = newtheme.OwnMessageColor,
                FriendMessageColor = newtheme.FriendMessageColor,
                ThemeName = newtheme.ThemeName,
                ConversationId = changeThemeRequest.ConversationId
            };
                
            foreach (var con in onlPar)
            {
                Clients.Client(con).SendAsync("ReceiverNewTheme", themeres);
                Clients.Client(con).SendAsync("ReceiverMessage", messageRes);
            }
            Clients.Caller.SendAsync("ReceiverMessage", messageRes);
        }
    }
}
