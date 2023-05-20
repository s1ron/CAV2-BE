namespace ChatAppBackEndV1.Data.Entities
{
    public class MessageReaction
    {
        public long Id { get; set; }
        public string ReactionType { get; set; }
        public long MessageId { get; set; }
        public Guid UserId { get; set; }

        //config

        public AppUser User { get; set; }
        public Message Message { get; set; }    
    }
}
