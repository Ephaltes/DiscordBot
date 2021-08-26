using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordBot.Core.Entity;
using DiscordBot.Core.Interfaces;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using Serilog;
using TimeSpanParserUtil;

namespace DiscordBot.Core.Commands
{
    public class CreateEventCommand : ApplicationCommandModule
    {
        private readonly IEventRepository _eventRepository;
        private readonly ILogger _logger;
        private readonly ITimeRepository _timeRepository;

        public CreateEventCommand(IEventRepository eventRepository, ILogger logger, ITimeRepository timeRepository)
        {
            _eventRepository = eventRepository;
            _logger = logger;
            _timeRepository = timeRepository;
        }

        [SlashRequireUserPermissions(Permissions.Administrator)]
        [SlashCommand("CreateEvent", "Creates a new Event")]
        public async Task CreateEvent(InteractionContext context,
            [Option("Name", "Name of the Event")] string eventName,
            [Option("Date", "Date of the Event")] string eventDate,
            [Option("Time", "Time of the Event")] string eventTime,
            [Option("ChannelToPost", "Channel To Post the Event")]
            DiscordChannel eventChannel,
            [Option("Reminders", "Remind of Event before it happends e.g. 1d,2d,2h")]
            string eventReminder = "")
        {
            try
            {
                await context.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                if (string.IsNullOrEmpty(eventDate) || !DateTime.TryParse(eventDate, out DateTime parsedDate))
                {
                    await context.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Wrong Date Format"));

                    return;
                }

                if (string.IsNullOrEmpty(eventTime) || !TimeSpan.TryParse(eventTime, out TimeSpan parsedTime))
                {
                    await context.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Wrong Time Format"));

                    return;
                }

                if (parsedDate.Add(parsedTime) < DateTime.Now)
                {
                    await context.EditResponseAsync(
                        new DiscordWebhookBuilder().WithContent("Date has to be in the future"));

                    return;
                }

                if (string.IsNullOrEmpty(eventName))
                {
                    await context.EditResponseAsync(new DiscordWebhookBuilder().WithContent("EventName is empty"));

                    return;
                }

                if (eventChannel.Type != ChannelType.Text)
                {
                    await context.EditResponseAsync(
                        new DiscordWebhookBuilder().WithContent("Channel is not a TextChannel"));

                    return;
                }

                char[] delimiterChars = { ' ', ',', '.' };

                string[] reminderSplitted = eventReminder.Split(delimiterChars,
                    StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

                List<TimeEntity> reminderList = new List<TimeEntity>();

                foreach (string reminder in reminderSplitted)
                {
                    if (!TimeSpanParser.TryParse(reminder, out TimeSpan parsedReminderTime))
                    {
                        await context.EditResponseAsync(
                            new DiscordWebhookBuilder().WithContent("Wrong Reminder Format"));

                        return;
                    }

                    TimeEntity timeEntity = await _timeRepository.GetByTimeSpan(parsedReminderTime) ??
                                            new TimeEntity { Time = parsedReminderTime };

                    reminderList.Add(timeEntity);
                }


                EventEntity entity = new EventEntity
                {
                    ServerId = context.Guild.Id,
                    Date = parsedDate.Add(parsedTime),
                    Name = eventName,
                    ChannelToPostId = eventChannel.Id,
                    TimeEntities = reminderList
                };

                await _eventRepository.Add(entity);

                await context.EditResponseAsync(
                    new DiscordWebhookBuilder().WithContent($"Event '{entity.Name}' added successful"));
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                _logger.Error(e.StackTrace);
            }
        }
    }
}