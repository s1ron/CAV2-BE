using ChatAppBackEndV2.Data.Enums;
using Microsoft.AspNetCore.Identity;

namespace ChatAppBackEndV1.Data.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FisrtName { get; set; }
        public string LastName { get; set; }
        public DateTime Dob { get; set; }
        public GenderEnum Gender { get; set; } = GenderEnum.Other;
        public string? ProfileImagePath { get; set; }
        public string? ProfileDescription { get; set; }

        //config foreignkey
        public List<FriendRequest> FriendRequestSenders { get; set; }
        public List<FriendRequest> FriendRequestReceivers { get; set; }
        public List<Friend> FriendUsers { get; set; }
        public List<Friend> FriendFriends { get; set; }
        public List<Participant> Participants { get; set; }
        //public List<FavoriteConversation> FavoriteConversations { get; set; }
        public List<MessageReaction> MessageReactions { get; set;}
        public List<Message> Messages { get; set; }


    }
}
