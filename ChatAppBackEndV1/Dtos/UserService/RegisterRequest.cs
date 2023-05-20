using ChatAppBackEndV2.Data.Enums;

namespace ChatAppBackEndV2.Dtos.UserService
{
    public class RegisterRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public GenderEnum Gender { get; set; }
        public string UserName { get; set; }
        public DateTime Dob { set; get; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}
