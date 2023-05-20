namespace ChatAppBackEndV1.Data.Entities
{
    public class Attachment
    {
        public long Id { get; set; }
        public long MessageId { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }

        //config
        public Message Message { get; set; }

    }
}
