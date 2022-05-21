using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using DiscordBot.Core.Entity;
using DiscordBot.Core.Helper;
using DiscordBot.Core.Interfaces;
using DSharpPlus;
using DSharpPlus.Entities;
using Humanizer;
using Serilog;

namespace DiscordBot.Core.Handler
{
    public class DiscordEventHandler : IDiscordEventHandler
    {
        private readonly DiscordClient _client;
        private readonly IEventRepository _eventRepository;
        private readonly Timer _eventTimer;
        private readonly ILogger _logger;
        public DiscordEventHandler(IEventRepository eventRepository, ILogger logger, DiscordClient client)
        {
            _eventRepository = eventRepository;
            _logger = logger.ForContext(GetType());
            _client = client;

            _eventTimer = new Timer();
            _eventTimer.AutoReset = false;
            _eventTimer.Interval = 1000 * 15; // 15 sekunden
            _eventTimer.Elapsed += CheckForEvents;
        }

        public async Task StartPolling()
        {
            _eventTimer.Start();
            await Task.Delay(-1);
        }

        public async void CheckForEvents(object? source, ElapsedEventArgs e)
        {
            _eventTimer.Stop();
            try
            {
                List<EventEntity> eventList = await _eventRepository.GetAll();

                if (eventList.Count < 1)
                {
                    _eventTimer.Start();

                    return;
                }

                foreach (var eventEntity in eventList)
                {
                    if (!IsTimeToSendMessageForEvent(eventEntity))
                        continue;

                    DiscordEmbed? message = CreateMessageForEvent(eventEntity);

                    if (message != null)
                        await SendMessageToChannel(eventEntity, message);
                }

                await DeleteExpiredEventsAndReminders(eventList);
            }
            catch (Exception exception)
            {
                _logger.Error(exception, exception.Message);
            }

            _eventTimer.Start();
        }

        public bool IsTimeToSendMessageForEvent(EventEntity entity)
        {
            if (entity.Date >= DateTime.Now &&
                entity.TimeEntities.FirstOrDefault(time => DateTime.Now.Add(time.Time) >= entity.Date) == null)
                return false;

            return true;
        }

        public DiscordEmbed? CreateMessageForEvent(EventEntity entity)
        {
            if (entity.Date <= DateTime.Now)
                return EmbeddedHelper.CreateDefaultEmbed($"{entity.Name} is now!");

            foreach (var reminderTime in entity.TimeEntities.OrderByDescending(x => x.Time))
            {
                string reminderString =
                    $"Event '{entity.Name}' is in about {reminderTime.Time.Humanize()} !\n" +
                    $"on {entity.Date.ToShortDateString()} " +
                    $"{entity.Date.ToShortTimeString()}";

                if (entity.Date > DateTime.Now.Add(reminderTime.Time))
                    continue;

                return EmbeddedHelper.CreateDefaultEmbed(reminderString);
            }

            return null;
        }

        public async Task DeleteExpiredEventsAndReminders(
            ICollection<EventEntity> entityList)
        {
            foreach (EventEntity entity in entityList)
            {
                if (entity.Date <= DateTime.Now)
                {
                    await _eventRepository.Delete(entity);

                    continue;
                }

                foreach (var reminderTime in entity.TimeEntities.ToList())
                {
                    if (entity.Date > DateTime.Now.Add(reminderTime.Time))
                        continue;

                    entity.TimeEntities.Remove(reminderTime);
                    await _eventRepository.Update(entity);
                }
            }
        }

        public async Task SendMessageToChannel(EventEntity entity, DiscordEmbed discordEmbed)
        {
            DiscordGuild? guild = _client.Guilds.FirstOrDefault(dictionary => dictionary.Key == entity.ServerId).Value;

            DiscordChannel? channel =
                guild.Channels.FirstOrDefault(dictionary => dictionary.Key == entity.ChannelToPostId).Value;

            await channel.SendMessageAsync(discordEmbed);
        }
    }
}