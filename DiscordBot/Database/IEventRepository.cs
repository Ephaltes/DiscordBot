using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordBot.Entity;

namespace DiscordBot.Database
{
    #nullable enable
    public interface IEventRepository
    {
        public Task<EventEntity?> Get(int id);

        public Task<bool> Add(EventEntity entity);

        public Task<bool> Update(EventEntity entity);

        public Task<bool> Delete(EventEntity entity);
    }
}