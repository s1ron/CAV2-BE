using ChatAppBackEndV1.Common.ServiceResponseResult;
using ChatAppBackEndV2.Dtos.UserService;

namespace ChatAppBackEndV1.Services.UserService
{
    public interface IUserService
    {
        Task<ResponseResult<GetUserResponse>> GetUserAsync(Guid userId);
        Task<ResponseResult<AuthenticateResponse>> AuthenticateAsync(string username, string password);
        Task<ResponseResult<bool>> RegisterAsync(RegisterRequest request);
        Task<ResponseResult<AuthenticateResponse>> GoogleLoginAsync(string providerName, string credential);
    }
}
