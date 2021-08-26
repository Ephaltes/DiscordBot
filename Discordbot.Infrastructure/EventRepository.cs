using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordBot.Core.Entity;
using DiscordBot.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Discordbot.Infrastructure
{
#nullable enable
    public class EventRepository : IEventRepository
    {
        private readonly DatabaseContext _db;
        private readonly ILogger _logger;
        public EventRepository(DatabaseContext db, ILogger logger)
        {
            _db = db;
            _logger = logger;
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
                
                foreach (TimeEntity timeEntity in entity.TimeEntities)
                {
                    if (timeEntity.Id > 0)
                        _db.Attach(timeEntity);
                }
                
                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);

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
                _logger.Error(e, e.Message);

                return false;
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            try
            {
                EventEntity? entity = await _db.EventEntities.FindAsync(id);

                if (entity == null)
                    return true;

                _db.EventEntities.Remove(entity);
                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);

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
                _logger.Error(e, e.Message);

                return false;
            }
        }
    }
}