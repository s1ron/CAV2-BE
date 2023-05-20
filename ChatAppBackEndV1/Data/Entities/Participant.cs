namespace ChatAppBackEndV1.Data.Entities
{
    public class Participant
    {
        public long Id { get; set; }
        public long ConversationId { get; set; }
        public Guid UserId { get; set; }
        public long? LastReadMessageId { get; set; }
        public string? NickName { get; set; }
        public bool IsFavoriteConversation { get; set; }

        //config
        public AppUser User { get; set; }
        public Conversation Conversation { get; set; }
    }
}
