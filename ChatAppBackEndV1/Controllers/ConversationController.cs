using ChatAppBackEndV2.Dtos.FriendService;
using ChatAppBackEndV2.Services.ConversationService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatAppBackEndV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationController : ControllerBase
    {
        private readonly IConversationService _conversationService;
        public ConversationController(IConversationService conversationService)
        {
            _conversationService= conversationService;
        }

        [HttpPost("getOrCreateConversation")]
        public async Task<IActionResult> GetOrCreateConversation([FromBody]SendFriendRequestResquest resquest)
        {
            var a = await _conversationService.GetOrCreateConversation(resquest.SenderId, resquest.ReceiverId);
            return Ok(a);
        }
    }
}
