using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordBot.Core.Entity;

namespace DiscordBot.Core.Interfaces
{
#nullable enable
    public interface IEventRepository
    {
        public Task<EventEntity?> Get(Guid id);

        public Task<List<EventEntity>> GetAll();

        public Task<bool> Add(EventEntity entity);

        public Task<bool> Update(EventEntity entity);

        public Task<bool> Delete(Guid id);

        public Task<bool> Delete(EventEntity entity);
    }
}