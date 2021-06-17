using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using DiscordBot.Entity;

namespace DiscordBot.Database
{
    #nullable enable
    public class EventRepository : IEventRepository
    {
        private readonly DatabaseContext _db;
        public EventRepository(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<EventEntity?> Get(int id)
        {
            return await _db.EventEntities.FindAsync(id);
        }

        public async Task<bool> Add(EventEntity entity)
        {
            try
            {
                await _db.EventEntities.AddAsync(entity);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                await LoggingService.Log(new LogMessage(LogSeverity.Error, nameof(EventRepository), e.Message, e));
                return false;
            }
        }

        public async Task<bool> Update(EventEntity entity)
        {
            try
            {
                _db.EventEntities.Update(entity);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                await LoggingService.Log(new LogMessage(LogSeverity.Error, nameof(EventRepository), e.Message, e));
                return false;
            }
        }

        public async Task<bool> Delete(EventEntity entity)
        {
            try
            {
                _db.EventEntities.Remove(entity);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                await LoggingService.Log(new LogMessage(LogSeverity.Error, nameof(EventRepository), e.Message, e));
                return false;
            }
        }
    }
}