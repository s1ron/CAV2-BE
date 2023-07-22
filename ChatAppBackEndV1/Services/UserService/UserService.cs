using ChatAppBackEndV1.Common.ServiceResponseResult;
using ChatAppBackEndV1.Data.Entities;
using ChatAppBackEndV2.Data.Enums;
using ChatAppBackEndV2.Dtos.ChatHubDtos;
using ChatAppBackEndV2.Dtos.UserService;
using ChatAppBackEndV2.Hubs;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChatAppBackEndV1.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _config;
        private readonly ChatHub _chatHub;

        public UserService(UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IConfiguration config,
            ChatHub chatHub)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
            _chatHub= chatHub;
        }
        public async Task<ResponseResult<AuthenticateResponse>> AuthenticateAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) { return new ErrorResponseResult<AuthenticateResponse>("Tài khoản không tồn tại"); }
            var result = await _signInManager.PasswordSignInAsync(user, password, true, true);
            if (!result.Succeeded)
            {
                return new ErrorResponseResult<AuthenticateResponse>("Mật khẩu không đúng");
            }
            if (_chatHub.CheckUserConnected(user.Id))
            {
                return new ErrorResponseResult<AuthenticateResponse>("Tài khoản đang đăng nhập trên thiết bị khác");
            }
            var authenresult = new AuthenticateResponse()
            {
                AccessToken = CreateToken(user),
                UserId = user.Id,
            };
            return new SuccessResponseResult<AuthenticateResponse>(authenresult);
        }

        private string CreateToken(AppUser user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                //new Claim(ClaimTypes.Name, user.UserName),
                new Claim("Id", user.Id.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _config["Tokens:Issuer"],
                _config["Tokens:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: creds);
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
            return accessToken;
        }

        public async Task<ResponseResult<GetUserResponse>> GetUserAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if(user == null)
            {
                return new ErrorResponseResult<GetUserResponse>("Not found user");
            }
            var res = new GetUserResponse()
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Dob = user.Dob,
                Gender = user.Gender,
                ProfileImagePath = user.ProfileImagePath,
                ProfileDescription = user.ProfileDescription,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email
            };
            return new SuccessResponseResult<GetUserResponse>(res);
        }

        public async Task<ResponseResult<bool>> RegisterAsync(RegisterRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user != null)
                return new ErrorResponseResult<bool>("Tài khoản đã tồn tại");
            if (await _userManager.FindByEmailAsync(request.Email) != null)
                return new ErrorResponseResult<bool>("Email đã tồn tại");
            user = new AppUser()
            {
                Dob = request.Dob,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Gender = (GenderEnum)Enum.Parse(typeof(GenderEnum), request.Gender),
                UserName = request.UserName,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                return new SuccessResponseResult<bool>();
            }
            return new ErrorResponseResult<bool>("Đăng ký không thành công");
        }

        public async Task<ResponseResult<AuthenticateResponse>> GoogleLoginAsync(string providerName, string credential)
        {
            var validateSettings = new GoogleJsonWebSignature.ValidationSettings
            {
                IssuedAtClockTolerance = TimeSpan.FromMinutes(5),
                ExpirationTimeClockTolerance = TimeSpan.FromMinutes(5),
            };
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(credential, validateSettings);
                var loginInfo = new UserLoginInfo(providerName, payload.Subject, providerName);
                var user = await _userManager.FindByLoginAsync(loginInfo.LoginProvider, loginInfo.ProviderKey);
                if (user == null)
                {
                    user = await _userManager.FindByEmailAsync(payload.Email);
                    if (user == null)
                    {
                        user = new AppUser()
                        {
                            Email = payload.Email,
                            FirstName = payload.FamilyName,
                            LastName = payload.GivenName,
                            ProfileImagePath = payload.Picture,
                            EmailConfirmed = payload.EmailVerified,
                            UserName = payload.Email.Split('@')[0],
                        };
                        var a = await _userManager.CreateAsync(user);
                        await _userManager.AddLoginAsync(user, loginInfo);
                    }
                    else
                    {
                        await _userManager.AddLoginAsync(user, loginInfo);
                    }
                }
                var result = new AuthenticateResponse()
                {
                    AccessToken = CreateToken(user),
                    UserId = user.Id,
                };
                return new SuccessResponseResult<AuthenticateResponse>(result);
            }
            catch (InvalidJwtException ex)
            {
                return new ErrorResponseResult<AuthenticateResponse>(ex.Message);
            }
        }
    }
}
