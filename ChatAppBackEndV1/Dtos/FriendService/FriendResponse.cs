using ChatAppBackEndV2.Data.Enums;

namespace ChatAppBackEndV2.Dtos.FriendService
{
    public class FriendResponse
    {
        public Guid FriendId { get; set; }
        public DateTime AddAt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Dob { get; set; }
        public GenderEnum? Gender { get; set; }
        public string? ProfileImagePath { get; set; }
        public string? ProfileDescription { get; set; }
    }
}

