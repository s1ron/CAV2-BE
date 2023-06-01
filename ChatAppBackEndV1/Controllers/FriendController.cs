using ChatAppBackEndV1.Services.FriendService;
using ChatAppBackEndV2.Dtos.FriendService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace ChatAppBackEndV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendController : ControllerBase
    {
        private readonly IFriendService _friendService;
        public FriendController(IFriendService friendService)
        {
            _friendService= friendService;
        }

        [HttpPost("acceptFriend")]
        public async Task<IActionResult> AcceptFriendRequest([FromBody] AcceptFriendRequestRequest acceptFriend)
        {
            var a = await _friendService.AcceptFriendRequestAsync(acceptFriend);
            if (a.IsSuccess)
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpPost("sendFriendRequest")]
        public async Task<IActionResult> SendFriendRequest([FromBody] SendFriendRequestResquest sendFriendRequestResquest)
        {
            var a = await _friendService.SendFriendRequestAsync(sendFriendRequestResquest.SenderId, sendFriendRequestResquest.ReceiverId);
            if (a.IsSuccess)
            {
                return Ok(a.Result);
            }
            return BadRequest();
        }

        [HttpPost("removeFriends")]
        public async Task<IActionResult> RemoveFriends([FromBody] SendFriendRequestResquest sendFriendRequestResquest)
        {
            var a = await _friendService.RemoveFriendAsync(sendFriendRequestResquest.SenderId, sendFriendRequestResquest.ReceiverId);
            if (a.IsSuccess)
            { 
                return Ok();
            }
            return BadRequest();
        }
    }
}
