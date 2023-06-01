namespace ChatAppBackEndV2.Dtos.ChatHubDtos
{
    public class ChangeEmojiRequest
    {
        public Guid SenderId { get; set; }
        public long ConversationId { get; set; }
        public string NewEmoji { get; set; }
    }
}
