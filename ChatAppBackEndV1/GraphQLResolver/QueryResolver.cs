using ChatAppBackEndV1.Common.ServiceResponseResult;
using ChatAppBackEndV1.Data.Entities;
using ChatAppBackEndV1.Dtos.FriendService;
using ChatAppBackEndV1.Services.FriendService;
using ChatAppBackEndV1.Services.UserService;
using ChatAppBackEndV2.Dtos.ConversationService;
using ChatAppBackEndV2.Dtos.FriendService;
using ChatAppBackEndV2.Dtos.MessageService;
using ChatAppBackEndV2.Dtos.UserService;
using ChatAppBackEndV2.Hubs;
using ChatAppBackEndV2.Services.ConversationService;
using ChatAppBackEndV2.Services.MessageService;
using ChatAppBackEndV2.Services.SystemImageService;
using Microsoft.EntityFrameworkCore;

namespace ChatAppBackEndV2.GraphQLResolver
{
    [ExtendObjectType("Query")]
    public class QueryResolver
    {
        [GraphQLName("GetCollapseConversations")]
        [GraphQLDescription("GetCollapseConversations")]
        public async Task<List<CollapseConversationResponse>> GetCollapseConversationsAsync(Guid userId, [Service]IConversationService conversationService)
        {
            var a = await conversationService.GetCollapseConversationsAsync(userId);
            return a;
        }

        [GraphQLName("GetCollapseConversationByConversationId")]
        [GraphQLDescription("GetCollapseConversationByConversationId")]
        public async Task<CollapseConversationResponse> GetCollapseConversationByConversationIdAsync(Guid userId, long conversationId, [Service] IConversationService conversationService)
        {
            var a = await conversationService.GetCollapseConversationByConversationIdAsync(userId, conversationId);
            return a;
        }

        [GraphQLName("GetFirstMessages")]
        [GraphQLDescription("GetFirstMessages")]
        public async Task<FirstMessageResponse> GetFirstMessageAsync(long conversationId, [Service]IMessageService messageService)
        {
            return await messageService.GetFirstMessageAsync(conversationId);
        }

        [GraphQLName("GetContinueMessage")]
        [GraphQLDescription("GetContinueMessage")]
        public async Task<List<SingleMessageResponse>> GetContinueMessage(long conversationId, long lastMessageId, [Service] IMessageService messageService)
        {
            return await messageService.GetContinueMessage(conversationId, lastMessageId);
        }

        [GraphQLName("GetUserById")]
        [GraphQLDescription("GetUserById")]
        public async Task<GetUserResponse> GetUserByIdAsync(Guid userId, [Service] IUserService userService)
        {
            var a = await userService.GetUserAsync(userId);
            if (a.IsSuccess)
            {
                return a.Result;
            }
            throw new GraphQLException(new Error(a.Message, "USER_NOT_FOUND"));
        }

        [GraphQLName("GetUserAndFriendStatus")]
        [GraphQLDescription("GetUserAndFriendStatus")]
        public async Task<GetUserByIdResponse> GetUserAndFriendStatusByIdAsync(Guid userId, Guid friendId, [Service] IFriendService friendService)
        {
            var a = await friendService.GetUserById(userId, friendId);
            if (a.IsSuccess)
            {
                return a.Result;
            }
            throw new GraphQLException(new Error(a.Message, "USER_NOT_FOUND"));
        }

        [GraphQLName("CheckFriendStatus")]
        [GraphQLDescription("CheckFriendStatus")]
        public async Task<FriendStatusResponse> CheckFriendStatusAsync(Guid userId, Guid friendId, [Service] IFriendService friendService)
        {
            return await friendService.CheckFriendStatusAsync(userId, friendId);

        }

        [GraphQLName("GetOnlineFriends")]
        [GraphQLDescription("GetOnlineFriends")]
        public async Task<List<FriendResponse>> GetOnlineFriendsAsync(Guid userId, [Service] IFriendService friendService, [Service] ChatHub chatHub)
        {
            var a = await friendService.GetFriendsAsync(userId);
            if (a.IsSuccess)
            {
                var b = chatHub.GetOnlineUsers(a.Result);
                return b;
            }
            return null;
        }

        [GraphQLName("GetFriends")]
        [GraphQLDescription("GetFriends")]
        public async Task<List<FriendResponse>> GetFriendsAsync(Guid userId, [Service] IFriendService friendService)
        {
            var a = await friendService.GetFriendsAsync(userId);
            if (a.IsSuccess)
            {
                return a.Result;
            }
            return null;
        }
        [GraphQLName("FindUsersByKeyword")]
        [GraphQLDescription("FindUsersByKeyword")]
        public async Task<List<AppUser>> FindUserByKeyword (string keyWord , [Service] IFriendService friendService)
        {
            var a = await friendService.FindUsersByKeyword(keyWord);
            if (a.IsSuccess)
            {
                return a.Result;
            }
            return null;
        }

        [GraphQLName("GetFriendRequest")]
        [GraphQLDescription("GetFriendRequest")]
        public async Task<List<FriendRequestResponse>> GetFriendRequestAsync(Guid userId, [Service] IFriendService friendService)
        {
            var a = await friendService.GetFriendRequestAsync(userId);
            if (a.IsSuccess)
            {
                return a.Result;
            }
            return null;
        }

        [GraphQLName("GetSystemImages")]
        [GraphQLDescription("GetSystemImages")]
        public Task<List<SystemImageMessage>> GetSystemImagesAsync(string type, [Service] ISystemImageService systemImageService)
        {
            return systemImageService.GetSystemImagesAsync(type);
        }

        [GraphQLName("GetListConversationTheme")]
        [GraphQLDescription("GetListConversationTheme")]
        public async Task<List<ConversationTheme>> GetListConversationTheme([Service] IConversationService conversationService)
        {
            return await conversationService.GetListConversationTheme();
        }

        [GraphQLName("GetConversationAttachment")]
        [GraphQLDescription("GetConversationAttachment")]
        public async Task<List<Attachment>> GetConversationAttachment(long conversationId, [Service] IConversationService conversationService)
        {
            return await conversationService.GetConversationAttachment(conversationId);
        }
    }
}
