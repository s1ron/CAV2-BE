using ChatAppBackEndV2.Data.Enums;

namespace ChatAppBackEndV1.Data.Entities
{
    public class Message
    {
        public long Id { get; set; }
        public long ConversationId { get; set; }
        public Guid SenderId { get; set; }
        public string Content { get; set; }
        public MessageTypeEnum MessageType { get; set; } = MessageTypeEnum.OTHER;
        public DateTime SendAt { get; set; }
        public string? NonFilePath { get; set; }


        //config
        public List<Attachment> Attachments { get; set; }
        public List<MessageReaction> MessageReactions { get; set; }
        public Conversation Conversation { get; set; }
        public AppUser User { get; set; }

    }
}
