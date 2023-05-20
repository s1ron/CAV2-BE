namespace ChatAppBackEndV2.Dtos.MessageService
{
    public class UploadMessageFile
    {
        public IFormFile File { get; set; }
        public long ConversationId { get; set; }
    }
}
