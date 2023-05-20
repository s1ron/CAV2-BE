using ChatAppBackEndV1.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatAppBackEndV1.Data.Configurations
{
    public class SystemImageMessageConfiguration : IEntityTypeConfiguration<SystemImageMessage>
    {
        public void Configure(EntityTypeBuilder<SystemImageMessage> builder)
        {
            builder.ToTable("SystemImageMessages");
            builder.HasKey(x => x.Id);

        }
    }
}
