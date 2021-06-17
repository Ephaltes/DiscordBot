using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using DiscordBot.Entity;
using Microsoft.Extensions.Logging;

namespace DiscordBot.Database
{
    #nullable enable
    public class UploadOnlyRepository : IUploadOnlyRepository
    {
        private readonly DatabaseContext _db;

        public UploadOnlyRepository(DatabaseContext db)
        {
            _db = db;
        }

        public async Task<UploadOnlyEntity?> Get(int id)
        {
            return await _db.UploadOnlyEntities.FindAsync(id);
        }

        public async Task<UploadOnlyEntity?> GetByChannelId(ulong channelId)
        {
            return await _db.UploadOnlyEntities.AsAsyncEnumerable()
                .Where(a=> a.ChannelId == channelId)
                .FirstOrDefaultAsync();
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
                await LoggingService.Log(new LogMessage(LogSeverity.Error, nameof(UploadOnlyRepository), e.Message, e));
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
                await LoggingService.Log(new LogMessage(LogSeverity.Error, nameof(UploadOnlyRepository), e.Message, e));
                return false;
            }
        }

        public async Task<bool> Delete(UploadOnlyEntity entity)
        {
            try
            {
                _db.UploadOnlyEntities.Remove(entity);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                await LoggingService.Log(new LogMessage(LogSeverity.Error, nameof(UploadOnlyRepository), e.Message, e));
                return false;
            }
        }
    }
}