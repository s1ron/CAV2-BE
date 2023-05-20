using ChatAppBackEndV2.Data.Enums;

namespace ChatAppBackEndV1.Dtos.FriendService
{
    public class FriendRequestResponse
    {
        public long FriendRequestId { get; set; }
        public DateTime SendAt { get; set; }
        public Guid SenderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Dob { get; set; }
        public GenderEnum? Gender { get; set; }
        public string? ProfileImagePath { get; set; }
        public string? ProfileDescription { get; set; }
    }
}
