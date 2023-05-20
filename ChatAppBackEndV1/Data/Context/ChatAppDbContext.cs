using ChatAppBackEndV1.Data.Configurations;
using ChatAppBackEndV1.Data.Entities;
using ChatAppBackEndV2.Data.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatAppBackEndV1.Data.Context
{
    public class ChatAppDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        public ChatAppDbContext(DbContextOptions options) : base(options)
        {

        }
        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims");
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles").HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);
            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens").HasKey(x => x.UserId);
            modelBuilder.Entity<IdentityRole<Guid>>().ToTable("AppRoles");
            

            modelBuilder.ApplyConfiguration(new AppUserConfiguration());
            modelBuilder.ApplyConfiguration(new FriendRequestConfiguration());
            modelBuilder.ApplyConfiguration(new FriendConfiguration());
            modelBuilder.ApplyConfiguration(new ParticipantConfiguration());
            modelBuilder.ApplyConfiguration(new ConversationConfiguration());
            modelBuilder.ApplyConfiguration(new MessageReactionConfiguration());
            modelBuilder.ApplyConfiguration(new ConversationThemeConfiguration());
            modelBuilder.ApplyConfiguration(new AttachmentConfiguration());
            modelBuilder.ApplyConfiguration(new MessageConfiguration());
            modelBuilder.ApplyConfiguration(new SystemImageMessageConfiguration());


            var hasher = new PasswordHasher<AppUser>();
            modelBuilder.Entity<AppUser>().HasData(new AppUser
            {
                Id = new Guid("69BD714F-9576-45BA-B5B7-F00649BE00DE"),
                UserName = "anhvu03",
                NormalizedUserName = "ANHVU03",
                Email = "anhvu.siron@gmail.com",
                NormalizedEmail = "ANHVU.SIRON@GMAIL.COM",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, "12345678"),
                SecurityStamp = string.Empty,
                FisrtName = "Vo Anh",
                LastName = "Vu",
                Gender = GenderEnum.Male
            });
        }


        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Conversation> Conversations { get;set; }
        public DbSet<ConversationTheme> ConversationThemes { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<MessageReaction> MessagesReactions { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<SystemImageMessage> SystemImageMessages { get; set; }
    }
}
