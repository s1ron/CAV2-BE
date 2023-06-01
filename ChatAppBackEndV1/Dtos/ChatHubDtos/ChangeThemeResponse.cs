using ChatAppBackEndV1.Data.Entities;

namespace ChatAppBackEndV2.Dtos.ChatHubDtos
{
    public class ChangeThemeResponse : ConversationTheme
    {
        public long ConversationId { get; set; }
    }
}
