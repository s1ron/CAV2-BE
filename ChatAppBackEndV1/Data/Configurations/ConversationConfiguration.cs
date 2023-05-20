using ChatAppBackEndV1.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatAppBackEndV1.Data.Configurations
{
    public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
    {
        public void Configure(EntityTypeBuilder<Conversation> builder)
        {
            builder.ToTable("Conversations");
            builder.HasKey(x => x.Id);  
            builder.Property(x=>x.Id).UseIdentityColumn();
            builder.HasOne(x=>x.ConversationTheme).WithMany(x=>x.Conversations)
                .HasForeignKey(x=>x.ConversationThemeId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
