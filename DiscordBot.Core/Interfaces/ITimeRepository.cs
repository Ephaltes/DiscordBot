using System;
using System.Threading.Tasks;
using DiscordBot.Core.Entity;

namespace DiscordBot.Core.Interfaces
{
#nullable enable
    public interface ITimeRepository
    {
        public Task<TimeEntity?> GetByTimeSpan(TimeSpan timespan);
    }
}