using ChatAppBackEndV2.Dtos.MessageService;

namespace ChatAppBackEndV2.Services.MessageService
{
    public interface IMessageService
    {
        Task<FirstMessageResponse> GetFirstMessageAsync(long conversationId);
        Task<List<SingleMessageResponse>> GetContinueMessage(long conversationId, long lastMessageId);
    }
}
