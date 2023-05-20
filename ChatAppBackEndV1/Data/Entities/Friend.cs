namespace ChatAppBackEndV1.Data.Entities
{
    public class Friend
    {
        public long Id { get; set; }
        public Guid UserId { get; set; }
        public Guid FriendId { get; set; }
        public DateTime AddAt { get; set; }

        public AppUser UserUser { get; set; }
        public AppUser UserFriend { get; set; }
    }
}
