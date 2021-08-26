using System;
using System.Threading.Tasks;
using DiscordBot.Core.Entity;
using DiscordBot.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Discordbot.Infrastructure
{
#nullable enable
    public class TimeRepository : ITimeRepository
    {
        private readonly DatabaseContext _db;
        private readonly ILogger _logger;
        public TimeRepository(DatabaseContext db, ILogger logger)
        {
            _db = db;
            _logger = logger.ForContext(GetType());
        }

        public async Task<TimeEntity?> GetByTimeSpan(TimeSpan timespan)
        {
            return await _db.TimeEntities.FirstOrDefaultAsync(x => x.Time == timespan);
        }
    }
}