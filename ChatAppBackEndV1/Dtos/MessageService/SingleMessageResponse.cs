using ChatAppBackEndV2.Data.Enums;

namespace ChatAppBackEndV2.Dtos.MessageService
{
    public class SingleMessageResponse
    {
        public long? Id { get; set; }
        public long? ConversationId { get; set; }
        public Guid? SenderId { get; set; }
        public string? Content { get; set; }
        public MessageTypeEnum? MessageType { get; set; }
        public DateTime? SendAt { get; set; }
        public string? FilePath { get; set; }
        public float? FileSize { get; set; }
        public List<ReactionOfAMessage>? MessageReaction { get; set; }
    }
}
