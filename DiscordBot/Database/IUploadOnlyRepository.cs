using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordBot.Entity;

namespace DiscordBot.Database
{
    #nullable enable
    public interface IUploadOnlyRepository
    {
        public Task<UploadOnlyEntity?> Get(int id);
        public Task<UploadOnlyEntity?> GetByChannelId(ulong channelId);

        public Task<bool> Add(UploadOnlyEntity entity);

        public Task<bool> Update(UploadOnlyEntity entity);

        public Task<bool> Delete(UploadOnlyEntity entity);
    }
}