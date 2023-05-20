using ChatAppBackEndV1.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatAppBackEndV1.Data.Configurations
{
    public class ConversationThemeConfiguration : IEntityTypeConfiguration<ConversationTheme>
    {
        public void Configure(EntityTypeBuilder<ConversationTheme> builder)
        {
            builder.ToTable("ConversationThemes");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
        }
    }
}
