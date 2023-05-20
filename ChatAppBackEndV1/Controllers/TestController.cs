using ChatAppBackEndV1.Services.UserService;
using ChatAppBackEndV2.Dtos.UserService;
using ChatAppBackEndV2.Services.ConversationService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatAppBackEndV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConversationService _conversationService;
        public TestController(IUserService userService,
            IConversationService conversationService)
        {
            _userService= userService;
            this._conversationService= conversationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> register(RegisterRequest registerRequest)
        {
            var a = await _userService.RegisterAsync(registerRequest);
            if(a.IsSuccess)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost("login")]
        public async Task<IActionResult> login(string username, string pass)
        {
            var a = await _userService.AuthenticateAsync(username, pass);
            if (a.IsSuccess)
            {
                return Ok(a.Result);
            }
            return BadRequest();
        }
        [HttpGet("get")]
        public async Task<IActionResult> get(Guid username)
        {
            return Ok(_conversationService.GetCollapseConversationsAsync(username));
        }

        [HttpPost("TestDate")]
        public async Task<IActionResult> getdawd([FromBody]test date)
        {
            var a = date;
            var dsef= a.Date.ToLocalTime();
            return Ok(dsef);
        }

        public class test
        {
            public DateTime Date { get; set; }
            public string Name { get; set; }
        }
    }
}
