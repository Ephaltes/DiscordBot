using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using DiscordBot.Entity;
using Serilog;

namespace DiscordBot.Database
{
    #nullable enable
    public class EventRepository : IEventRepository
    {
        private readonly DatabaseContext _db;
        private readonly ILogger _logger = Log.ForContext<EventRepository>();
        public EventRepository(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<EventEntity?> Get(Guid id)
        {
            return await _db.EventEntities.FindAsync(id);
        }

        public async Task<List<EventEntity>> GetAll()
        {
            return await _db.EventEntities.ToListAsync();
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
                _logger.Error(e,e.Message);
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
                _logger.Error(e,e.Message);
                return false;
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            try
            {
                var entity = await _db.EventEntities.FindAsync(id);
                if (entity != null)
                {
                    _db.EventEntities.Remove(entity);
                    await _db.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e,e.Message);
                return false;
            }
        }
    }
}