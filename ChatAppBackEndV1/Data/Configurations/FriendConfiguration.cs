using ChatAppBackEndV1.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatAppBackEndV1.Data.Configurations
{
    public class FriendConfiguration : IEntityTypeConfiguration<Friend>
    {
        public void Configure(EntityTypeBuilder<Friend> builder)
        {
            builder.ToTable("Friends");
            builder.HasKey(x => x.Id);
            builder.Property(x=>x.Id).UseIdentityColumn();
            builder.HasIndex(x => new {x.UserId, x.FriendId});
            builder.HasOne(x=>x.UserFriend).WithMany(x=>x.FriendFriends)
                .HasForeignKey(x=>x.FriendId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x=>x.UserUser).WithMany(x=>x.FriendUsers)
                .HasForeignKey(x=>x.UserId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
