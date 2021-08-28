using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using DiscordBot.Core.Entity;
using DSharpPlus.Entities;

namespace DiscordBot.Core.Interfaces
{
    public interface IDiscordEventHandler
    {
        public Task StartPolling();
        public void CheckForEvents(object source, ElapsedEventArgs e);
        public bool IsTimeToSendMessageForEvent(EventEntity entity);
        public DiscordEmbed? CreateMessageForEvent(EventEntity entity);
        public Task DeleteExpiredEventsAndReminders(ICollection<EventEntity> entityList);
        public Task SendMessageToChannel(EventEntity entity, DiscordEmbed discordEmbed);
    }
}