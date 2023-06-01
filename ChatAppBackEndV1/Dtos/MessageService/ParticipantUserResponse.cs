using ChatAppBackEndV2.Data.Enums;

namespace ChatAppBackEndV2.Dtos.MessageService
{
    public class ParticipantUserResponse
    {
        public long? LastReadMessageId { get; set; }
        public string? NickName { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public GenderEnum? Gender { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? ProfileImagePath { get; set; }
        public string? ProfileDescription { set; get; }
        public bool IsFavoriteConversation { get; set; } = false;
    }
}
