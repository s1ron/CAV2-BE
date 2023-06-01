using ChatAppBackEndV1.Common.ServiceResponseResult;
using ChatAppBackEndV1.Data.Entities;
using ChatAppBackEndV1.Dtos.FriendService;
using ChatAppBackEndV2.Dtos.FriendService;

namespace ChatAppBackEndV1.Services.FriendService
{
    public interface IFriendService
    {
        Task<ResponseResult<long>> SendFriendRequestAsync(Guid senderId, Guid receiverId);
        Task<ResponseResult<List<FriendRequestResponse>>> GetFriendRequestAsync(Guid userId);
        Task<ResponseResult<bool>> AcceptFriendRequestAsync(AcceptFriendRequestRequest acceptFriend);
        Task<ResponseResult<bool>> RemoveFriendAsync(Guid userId, Guid friendId);
        Task<FriendStatusResponse> CheckFriendStatusAsync(Guid userId, Guid friendId);
        Task<ResponseResult<List<FriendResponse>>> GetFriendsAsync(Guid userId);
        Task<List<FriendResponse>> GetOnlineFriendsAsync(Guid userId);
        Task<ResponseResult<List<AppUser>>> FindUsersByKeyword(string keyWord);
        Task<ResponseResult<GetUserByIdResponse>>GetUserById(Guid userId, Guid friendId);
    }
}
