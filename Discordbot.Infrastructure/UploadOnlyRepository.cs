using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscordBot.Core.Entity;
using DiscordBot.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Discordbot.Infrastructure
{
#nullable enable
    public class UploadOnlyRepository : IUploadOnlyRepository
    {
        private readonly DatabaseContext _db;
        private readonly ILogger _logger;

        public UploadOnlyRepository(DatabaseContext db, ILogger logger)
        {
            _db = db;
            _logger = logger.ForContext(GetType());
        }

        public async Task<UploadOnlyEntity?> Get(Guid id)
        {
            return await _db.UploadOnlyEntities.FindAsync(id);
        }

        public async Task<List<UploadOnlyEntity>> GetAll()
        {
            return await _db.UploadOnlyEntities.ToListAsync();
        }

        public async Task<List<UploadOnlyEntity>> GetAllbyServerId(ulong serverid)
        {
            return await _db.UploadOnlyEntities.AsQueryable()
                .Where(x => x.ServerId == serverid).ToListAsync();
        }

        public async Task<UploadOnlyEntity?> GetByChannelId(ulong channelId)
        {
            return await _db.UploadOnlyEntities.AsQueryable()
                .FirstOrDefaultAsync(a => a.ChannelId == channelId);
        }
        public async Task<bool> Add(UploadOnlyEntity entity)
        {
            try
            {
                await _db.UploadOnlyEntities.AddAsync(entity);
                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);

                return false;
            }
        }

        public async Task<bool> Update(UploadOnlyEntity entity)
        {
            try
            {
                _db.UploadOnlyEntities.Update(entity);
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
                UploadOnlyEntity? entity = await _db.UploadOnlyEntities.FindAsync(id);
                if (entity != null)
                {
                    _db.UploadOnlyEntities.Remove(entity);
                    await _db.SaveChangesAsync();
                }

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