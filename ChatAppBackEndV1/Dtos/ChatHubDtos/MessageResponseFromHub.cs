using ChatAppBackEndV2.Data.Enums;

namespace ChatAppBackEndV2.Dtos.ChatHubDtos
{
    public class MessageResponseFromHub
    {
        public long? Id { get; set; }
        public long? ConversationId { get; set; }
        public string? SenderId { get; set; }
        public string? Content { get; set; }
        public string? MessageType { get; set; }
        public DateTime? SendAt { get; set; }
        public string? FilePath { get; set; }
        public float? FileSize { get; set; }
    }
}
