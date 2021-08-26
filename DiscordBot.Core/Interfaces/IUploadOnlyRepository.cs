using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordBot.Core.Entity;

namespace DiscordBot.Core.Interfaces
{
#nullable enable
    public interface IUploadOnlyRepository
    {
        public Task<UploadOnlyEntity?> Get(Guid id);
        public Task<List<UploadOnlyEntity>> GetAll();
        public Task<List<UploadOnlyEntity>> GetAllbyServerId(ulong serverid);
        public Task<UploadOnlyEntity?> GetByChannelId(ulong channelId);

        public Task<bool> Add(UploadOnlyEntity entity);

        public Task<bool> Update(UploadOnlyEntity entity);

        public Task<bool> Delete(Guid id);
    }
}