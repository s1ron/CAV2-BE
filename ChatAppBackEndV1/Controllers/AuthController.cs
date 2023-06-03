using ChatAppBackEndV1.Services.UserService;
using ChatAppBackEndV2.Dtos.UserService;
using Microsoft.AspNetCore.Mvc;

namespace ChatAppBackEndV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(LoginResquest loginResquest)
        {
            var loginresult = await _userService.AuthenticateAsync(loginResquest.Username, loginResquest.Pass);
            if (!loginresult.IsSuccess)
            {
                return Unauthorized(loginresult.Message);
            }
            return Ok(loginresult.Result);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            var registerresult = await _userService.RegisterAsync(registerRequest);
            if (!registerresult.IsSuccess)
            {
                return Unauthorized(registerresult.Message);
            }
            return Ok();
        }

        [HttpPost("GoogleLogin")]
        public async Task<IActionResult> GoogleLogin([FromBody]GoogleCredentialRequest credential)
        {
            var result = await _userService.GoogleLoginAsync("Google", credential.Credential);
            if(result.IsSuccess)
            {
                return Ok(result.Result);
            }
            return BadRequest(result.Message);
        }
    }
}
