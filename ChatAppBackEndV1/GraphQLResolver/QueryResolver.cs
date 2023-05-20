using ChatAppBackEndV1.Common.ServiceResponseResult;
using ChatAppBackEndV1.Services.FriendService;
using ChatAppBackEndV1.Services.UserService;
using ChatAppBackEndV2.Dtos.ConversationService;
using ChatAppBackEndV2.Dtos.FriendService;
using ChatAppBackEndV2.Dtos.MessageService;
using ChatAppBackEndV2.Dtos.UserService;
using ChatAppBackEndV2.Hubs;
using ChatAppBackEndV2.Services.ConversationService;
using ChatAppBackEndV2.Services.MessageService;

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

        [GraphQLName("GetFirstMessages")]
        [GraphQLDescription("GetFirstMessages")]
        public async Task<FirstMessageResponse> GetFirstMessageAsync(long conversationId, [Service]IMessageService messageService)
        {
            return await messageService.GetFirstMessageAsync(conversationId);
        }

        [GraphQLName("GetUserById")]
        [GraphQLDescription("GetUserById")]
        public async Task<GetUserResponse> GetUserByIdAsync(Guid userId, [Service] IUserService userService)
        {
            var result = await userService.GetUserAsync(userId);
            if (result.IsSuccess)
            {
                return result.Result;
            }
            throw new GraphQLException(new Error(result.Message, "USER_NOT_FOUND"));
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
    }
}
