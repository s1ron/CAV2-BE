using ChatAppBackEndV1.Data.Entities;

namespace ChatAppBackEndV2.Dtos.MessageService
{
    public class FirstMessageResponse
    {
        public bool IsGroup { get; set; }
        public long ConversationId { get; set; }
        public string? ConversationName { get; set; }
        public Guid? AuthorId { get; set; }
        public bool IsMessageRequest { get; set; }
        public string? ConversationImagePath { get; set; }
        public bool IsBlock { get; set; }
        public Guid? BlockBy { get; set; }
        public long? ConversationThemeId { get; set; }
        public string? QuickMessage { get; set; }

        public ConversationTheme? ConversationTheme { get; set; }
        public List<ParticipantUserResponse> Participants { get; set; }
        public List<SingleMessageResponse>? Messages { get; set; }
    }
}
