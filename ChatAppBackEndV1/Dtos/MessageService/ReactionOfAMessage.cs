namespace ChatAppBackEndV2.Dtos.MessageService
{
    public class ReactionOfAMessage
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string? ProfileImagePath { get; set; }
        public string? ReactionType { get; set; }
        public long MessageId { get; set; }
    }
}
