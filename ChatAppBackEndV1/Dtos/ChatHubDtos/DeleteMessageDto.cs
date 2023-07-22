namespace ChatAppBackEndV2.Dtos.ChatHubDtos
{
    public class DeleteMessageDto
    {
        public Guid SenderId { get; set; }
        public long ConversationId { get; set; }
        public long MessageId { get; set; }
    }
}
