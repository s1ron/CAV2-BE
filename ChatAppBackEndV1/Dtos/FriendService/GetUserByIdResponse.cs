using ChatAppBackEndV2.Data.Enums;
using System.Globalization;

namespace ChatAppBackEndV2.Dtos.FriendService
{
    public class GetUserByIdResponse
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? Dob { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public GenderEnum Gender { get; set; } = GenderEnum.OTHER;
        public string? ProfileImagePath { get; set; }
        public string? ProfileDescription { get; set; }
        public FriendStatusResponse FriendStatus { get; set; }

    }
}
