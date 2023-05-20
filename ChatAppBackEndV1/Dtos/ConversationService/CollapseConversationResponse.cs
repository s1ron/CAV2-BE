using ChatAppBackEndV1.Data.Entities;
using ChatAppBackEndV2.Dtos.MessageService;

namespace ChatAppBackEndV2.Dtos.ConversationService
{
    public class CollapseConversationResponse
    {
        public bool IsGroup { get; set; }
        public long ConversationId { get; set; }
        public string? ConversationName { get; set;}
        public Guid? AuthorId { get; set; }
        public bool IsMessageRequest { get; set; }
        public string? ConversationImagePath { get; set; }
        public bool IsBlock { get; set; }
        public Guid? BlockBy { get; set; }
        public long? ConversationThemeId { get; set; }
        //public string? NickName { get; set; }
        public string QuickMessage { get; set; }
        public DateTime CreateAt { get; set; }
        public Message? LastMessage { get; set; }
        public bool IsFavoriteConversation { get; set; }
        public List<ParticipantUserResponse> ParticipantUser { get; set; }
    }
}
