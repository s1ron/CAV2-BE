using ChatAppBackEndV2.Data.Enums;

namespace ChatAppBackEndV2.Dtos.ChatHubDtos
{
    public class SendMessageResquest
    {
        public string MessageType { get; set; }
        public string Content { get; set; }
        public Guid SenderId { get; set; }
        public long ConversationId { get; set; }
        public DateTime SendAt { get; set; }
        public string? FilePath { get; set; }
        public long? FileSize { get; set; }
    }
}
