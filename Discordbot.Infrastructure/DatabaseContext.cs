using DiscordBot.Core.Entity;
using Microsoft.EntityFrameworkCore;

namespace Discordbot.Infrastructure
{
    public class DatabaseContext : DbContext
    {
        public DbSet<UploadOnlyEntity> UploadOnlyEntities { get; set; }
        public DbSet<EventEntity> EventEntities { get; set; }

        public DbSet<TimeEntity> TimeEntities { get; set; }
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            Database.Migrate();
        }
    }
}