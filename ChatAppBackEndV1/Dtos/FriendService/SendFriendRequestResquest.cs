namespace ChatAppBackEndV2.Dtos.FriendService
{
    public class SendFriendRequestResquest
    {
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
    }
}
