using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordBot.Entity;

namespace DiscordBot.Database
{
    #nullable enable
    public interface IEventRepository
    {
        public Task<EventEntity?> Get(Guid id);

        public Task<List<EventEntity>> GetAll();

        public Task<bool> Add(EventEntity entity);

        public Task<bool> Update(EventEntity entity);

        public Task<bool> Delete(Guid id);
    }
}