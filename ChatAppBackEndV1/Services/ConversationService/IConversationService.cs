using ChatAppBackEndV1.Data.Entities;
using ChatAppBackEndV2.Dtos.ConversationService;

namespace ChatAppBackEndV2.Services.ConversationService
{
    public interface IConversationService
    {
        Task<List<CollapseConversationResponse>> GetCollapseConversationsAsync(Guid userId);
        Task<CollapseConversationResponse> GetCollapseConversationByConversationIdAsync(Guid userId, long conversationId);
        Task<long> GetOrCreateConversation(Guid userId, Guid friendId);
        Task<List<ConversationTheme>> GetListConversationTheme();
        Task<List<Attachment>> GetConversationAttachment(long conversationId);
    }
}
