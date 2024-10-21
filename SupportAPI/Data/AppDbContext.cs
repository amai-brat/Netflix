using Microsoft.EntityFrameworkCore;
using SupportAPI.Data.Entities;

namespace SupportAPI.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<SupportChatMessage> SupportChatMessages => Set<SupportChatMessage>();
        public DbSet<SupportChatSession> SupportChatSessions => Set<SupportChatSession>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SupportChatSession>()
                .HasMany<SupportChatMessage>()
                .WithOne(scm => scm.ChatSession)
                .HasForeignKey(scm => scm.ChatSessionId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
