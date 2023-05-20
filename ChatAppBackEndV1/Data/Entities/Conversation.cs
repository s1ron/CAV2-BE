namespace ChatAppBackEndV1.Data.Entities
{
    public class Conversation
    {
        public long Id { get; set; }
        public string? ConversationName { get; set; }
        public string? ConversationImagePath { get; set; }
        public Guid? AuthorId { get; set; }
        public DateTime CreateAt { get; set; }
        public bool IsGroup { get; set; }
        public bool IsMessageRequest { get; set; }
        public bool IsBlock { get; set; }
        public Guid? BlockBy { get; set; }
        public string QuickMessage { get; set; }
        public long ConversationThemeId { get; set; }


        //config

        public List<Participant> Participants { get; set; }
        public List<Message> Messages { get; set; }
        public ConversationTheme ConversationTheme { get; set; }
    }
}
