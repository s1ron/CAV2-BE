namespace ChatAppBackEndV2.Dtos.UserService
{
    public class UploadAvatarRequest
    {
        public IFormFile File { get; set; }
        public Guid UserId { get; set; }
    }
}
