using ChatAppBackEndV2.Dtos.ConversationService;

namespace ChatAppBackEndV2.Services.ConversationService
{
    public interface IConversationService
    {
        Task<List<CollapseConversationResponse>> GetCollapseConversationsAsync(Guid userId);
        Task<long> GetOrCreateConversation(Guid userId, Guid friendId);
    }
}
