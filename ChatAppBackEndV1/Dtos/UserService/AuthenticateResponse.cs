namespace ChatAppBackEndV2.Dtos.UserService
{
    public class AuthenticateResponse
    {
        public string AccessToken { get; set; }
        public Guid UserId { get; set; }
    }
}
