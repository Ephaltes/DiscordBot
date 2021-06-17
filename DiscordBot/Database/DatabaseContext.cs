using DiscordBot.Entity;
using Microsoft.EntityFrameworkCore;

namespace DiscordBot.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
            Database.Migrate();
        }
        
        public DbSet<UploadOnlyEntity> UploadOnlyEntities { get; set; }
        public DbSet<EventEntity> EventEntities { get; set; }
        
        public DbSet<TimeEntity> TimeEntities { get; set; }
        
    }
}