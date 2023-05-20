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
            //HttpContext httpContext = Context.GetHttpContext();
            var cid = Context.ConnectionId;
            var uid = Context.GetHttpContext().Request.Query["userid"].ToString();
            var id = new Guid(uid);
            _connectedUsers.Add(id, cid);
            foreach(var user in _connectedUsers)
            {
                Console.WriteLine(user.Key);
            }
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var a = _connectedUsers.FirstOrDefault(x=>x.Value == Context.ConnectionId);
            _connectedUsers.Remove(a.Key);
            return base.OnDisconnectedAsync(exception);
        }

        public async Task<long> Test(string message)
        {
            await Clients.All.SendAsync("ReceiverMessage", message);
            return 123;
        }

        public async Task<object> SendMessage(SendMessageResquest sendMessageResquest)
        {
            var message = new Message()
            {
                ConversationId = sendMessageResquest.ConversationId,
                SenderId = new Guid(sendMessageResquest.SenderId),
                Content = sendMessageResquest.Content,
                MessageType = (MessageTypeEnum)Enum.Parse(typeof(MessageTypeEnum), sendMessageResquest.MessageType),
                SendAt = sendMessageResquest.SendAt.ToLocalTime(),
            };
            //await _context.Messages.AddAsync(message);

            if (message.MessageType == MessageTypeEnum.IMAGE ||
                message.MessageType == MessageTypeEnum.FILE)
            {
                var att = new Attachment()
                {
                    MessageId = message.Id,
                    FilePath = sendMessageResquest.FilePath,
                    FileSize = (long)sendMessageResquest.FileSize
                };
                await _context.Attachments.AddAsync(att);
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
            //await Clients.All.SendAsync("ReceiverMessage", message);
            return new { message.Id, message.SendAt };
        }
    }
}
