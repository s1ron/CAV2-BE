namespace ChatAppBackEndV2.Dtos.FriendService
{
    public class AcceptFriendRequestRequest
    {
        public long FriendRequestId { get; set; }
        public bool IsAccept { get; set; }
    }
}
