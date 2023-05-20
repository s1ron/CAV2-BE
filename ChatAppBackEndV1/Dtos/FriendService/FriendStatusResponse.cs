namespace ChatAppBackEndV2.Dtos.FriendService
{
    public class FriendStatusResponse
    {
        public bool IsFriend { get; set; } = false;
        public bool IsSendingRequest { get; set; } = false;
        public bool IsReceiverRequest { get; set; } = false;
        public long FriendRequestId { get; set; } = 0;
    }
}
