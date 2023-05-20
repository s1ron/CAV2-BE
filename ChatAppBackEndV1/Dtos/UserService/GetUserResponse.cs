using ChatAppBackEndV2.Data.Enums;

namespace ChatAppBackEndV2.Dtos.UserService
{
    public class GetUserResponse
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public DateTime Dob { get; set; }
        public GenderEnum? Gender { get; set; }
        public string? ProfileImagePath { get; set; }
        public string? ProfileDescription { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
