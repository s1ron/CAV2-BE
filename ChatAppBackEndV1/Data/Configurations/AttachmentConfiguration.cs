using ChatAppBackEndV1.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatAppBackEndV1.Data.Configurations
{
    public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
    {
        public void Configure(EntityTypeBuilder<Attachment> builder)
        {
            builder.ToTable("Attachments");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.HasIndex(x => x.MessageId);
            builder.HasOne(x=>x.Message).WithMany(x=>x.Attachments)
                .HasForeignKey(x => x.MessageId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
