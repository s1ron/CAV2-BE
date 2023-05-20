namespace ChatAppBackEndV1.Data.Entities
{
    public class ConversationTheme
    {
        public long Id { get; set; }
        public string ThemeName { get; set; }
        public string BgType { get; set; }
        public string BgColor {get; set;}
        public string OwnMessageColor { get; set;}
        public string FriendMessageColor { get; set; }

        //config
        public List<Conversation> Conversations { get; set;}
    }
}
