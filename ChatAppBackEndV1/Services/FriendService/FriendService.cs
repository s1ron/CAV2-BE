using ChatAppBackEndV1.Common.ServiceResponseResult;
using ChatAppBackEndV1.Data.Context;
using ChatAppBackEndV1.Data.Entities;
using ChatAppBackEndV1.Dtos.FriendService;
using ChatAppBackEndV2.Dtos.FriendService;
using ChatAppBackEndV2.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChatAppBackEndV1.Services.FriendService
{
    public class FriendService : IFriendService
    {
        private readonly ChatAppDbContext _context;
        private readonly ChatHub _chatHub;
        public FriendService(ChatAppDbContext context,
            ChatHub chatHub
            )
        {
            _context= context;
            _chatHub= chatHub;
        }

        public async Task<ResponseResult<bool>> AcceptFriendRequestAsync(AcceptFriendRequestRequest acceptFriend)
        {
            var friendRequest = await _context.FriendRequests.FindAsync(acceptFriend.FriendRequestId);
            if(friendRequest != null)
            {
                if (acceptFriend.IsAccept)
                {
                    var now = DateTime.Now;
                    List<Friend> friends = new()
                    {
                        new Friend() {
                            UserId = friendRequest.SenderId,
                            FriendId= friendRequest.ReceiverId,
                            AddAt = now,
                        },
                        new Friend()
                        {
                            UserId = friendRequest.ReceiverId,
                            FriendId = friendRequest.SenderId,
                            AddAt = now,
                        }
                    };
                    await _context.Friends.AddRangeAsync(friends);
                }
                _context.FriendRequests.Remove(friendRequest);
                await _context.SaveChangesAsync();
                return new SuccessResponseResult<bool>();
            }
            return new ErrorResponseResult<bool>();
        }

        public async Task<FriendStatusResponse> CheckFriendStatusAsync(Guid userId, Guid friendId)
        {
            var friend = await _context.Friends.FirstOrDefaultAsync(x=>(x.FriendId== friendId && x.UserId == userId)
                    ||(x.FriendId == userId && x.UserId == friendId));
            if(friend != null)
            {
                return new FriendStatusResponse() { IsFriend = true};
            }
            var friendsendingrequest = await _context.FriendRequests.FirstOrDefaultAsync(x=>x.SenderId == userId && x.ReceiverId== friendId);
            if (friendsendingrequest != null)
            {
                return new FriendStatusResponse() { IsSendingRequest = true, FriendRequestId = friendsendingrequest.Id };
            }
            var friendreceiverrequest = await _context.FriendRequests.FirstOrDefaultAsync(x => x.SenderId == friendId && x.ReceiverId == userId);
            if (friendreceiverrequest != null)
            {
                return new FriendStatusResponse() { IsReceiverRequest = true, FriendRequestId = friendreceiverrequest.Id };
            }
            return new FriendStatusResponse();
        }

        public async Task<ResponseResult<List<FriendResponse>>> GetFriendsAsync(Guid userId)
        {
            var friend = from fr in _context.Friends
                         join user in _context.Users on fr.FriendId equals user.Id
                         where fr.UserId == userId
                         select new {fr, user};

            if (friend == null) 
            {
                return new ErrorResponseResult<List<FriendResponse>>();
            }
            var result = await friend.Select(x => new FriendResponse()
            {
                FriendId = x.fr.FriendId,
                AddAt = x.fr.AddAt,
                FirstName = x.user.FirstName,
                LastName = x.user.LastName,
                Dob= x.user.Dob,
                ProfileDescription= x.user.ProfileDescription,
                ProfileImagePath= x.user.ProfileImagePath,
                Gender= x.user.Gender,
            }).ToListAsync();
            return new SuccessResponseResult<List<FriendResponse>>(result);
        }

        public async Task<ResponseResult<List<FriendRequestResponse>>> GetFriendRequestAsync(Guid userId)
        {
            var friendRequest = from fr in _context.FriendRequests
                                join user in _context.Users on fr.SenderId equals user.Id
                                where fr.ReceiverId == userId
                                select new { fr, user };
            if(friendRequest == null)
            {
                return new ErrorResponseResult<List<FriendRequestResponse>>();
            }
            var friendRequestlist = await friendRequest.Select(x => new FriendRequestResponse()
            {
                FriendRequestId = x.fr.Id,
                SendAt = x.fr.SendAt,
                SenderId = x.fr.SenderId,
                FirstName = x.user.FirstName,
                LastName= x.user.LastName,
                Dob = x.user.Dob,
                Gender = x.user.Gender,
                ProfileImagePath = x.user.ProfileImagePath,
                ProfileDescription = x.user.ProfileDescription
            }).ToListAsync();
            return new SuccessResponseResult<List<FriendRequestResponse>>(friendRequestlist);
        }

        public async Task<ResponseResult<bool>> RemoveFriendAsync(Guid userId, Guid friendId)
        {
            var user = await _context.Friends.Where(x=>(x.UserId == userId && x.FriendId == friendId) 
                || (x.UserId == friendId && x.FriendId == userId)).ToListAsync();
            
            if (user.Count == 2)
            {
                _context.RemoveRange(user);
                await _context.SaveChangesAsync();
                return new SuccessResponseResult<bool>();
            }
            return new ErrorResponseResult<bool>();
        }

        public async Task<ResponseResult<long>> SendFriendRequestAsync(Guid senderId, Guid receiverId)
        {
            var check = await _context.FriendRequests.Where(x => x.SenderId == senderId && x.ReceiverId == receiverId).ToListAsync();
            if (check.Count == 0)
            {
                var friendRequest = new FriendRequest()
                {
                    SenderId = senderId,
                    ReceiverId = receiverId,
                    SendAt = DateTime.Now
                };
                await _context.FriendRequests.AddAsync(friendRequest);
                await _context.SaveChangesAsync();
                return new SuccessResponseResult<long>(friendRequest.Id);
            }
            return new ErrorResponseResult<long>();
        }

        public async Task<List<FriendResponse>> GetOnlineFriendsAsync(Guid userId)
        {
            var a = await this.GetFriendsAsync(userId);
            if (a.IsSuccess)
            {
                var b = _chatHub.GetOnlineUsers(a.Result);
                return b;
            }
            return null;

        }

        public async Task<ResponseResult<List<AppUser>>> FindUsersByKeyword(string keyWord)
        {
            var a = await _context.Users.Where(x => String.Concat(x.FirstName, " " , x.LastName).Contains(keyWord)).ToListAsync();
            if (a == null)
            {
                return new ErrorResponseResult<List<AppUser>>();
            }
            return new SuccessResponseResult<List<AppUser>>(a);
        }

        public async Task<ResponseResult<GetUserByIdResponse>> GetUserById(Guid userId, Guid friendId)
        {
            var findedUser = await _context.Users.FindAsync(friendId);
            if (findedUser == null)
            {
                return new ErrorResponseResult<GetUserByIdResponse>();
            }
            var status = await CheckFriendStatusAsync(userId, friendId);
            var user = new GetUserByIdResponse()
            {
                FirstName= findedUser.FirstName,
                LastName= findedUser.LastName,
                Dob = findedUser.Dob,
                Email = findedUser.Email,
                PhoneNumber = findedUser.PhoneNumber,
                UserId= userId,
                Gender= findedUser.Gender,
                ProfileDescription= findedUser.ProfileDescription,
                ProfileImagePath= findedUser.ProfileImagePath,
                FriendStatus= status,
            };
            return new SuccessResponseResult<GetUserByIdResponse>(user);
        }
    }
}
