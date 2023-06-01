namespace ChatAppBackEndV2.Dtos.ChatHubDtos
{
    public class ChangeThemeRequest
    {
        public Guid SenderId { get; set; }
        public long ConversationId { get; set; }
        public long NewThemeId { get; set; }
    }
}
