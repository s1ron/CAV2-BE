namespace ChatAppBackEndV1.Data.Entities
{
    public class FriendRequest
    {
        public long Id { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public DateTime SendAt { get; set; }


        //config key
        public AppUser Sender { get; set; }
        public AppUser Receiver { get; set; }
    }
}
