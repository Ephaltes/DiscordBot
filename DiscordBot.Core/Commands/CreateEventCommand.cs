using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordBot.Core.Entity;
using DiscordBot.Core.Extension;
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
            _logger = logger.ForContext(GetType());
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
                string errorMessage;
                await context.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
                _logger.LogCallerInformation(context);

                if (string.IsNullOrEmpty(eventDate) || !DateTime.TryParse(eventDate, out DateTime parsedDate))
                {
                    errorMessage = "Wrong Date Format";
                    await context.EditResponseAsync(new DiscordWebhookBuilder().WithContent(errorMessage));
                    _logger.LogCallerInformation(context, errorMessage);

                    return;
                }

                if (string.IsNullOrEmpty(eventTime) || !TimeSpan.TryParse(eventTime, out TimeSpan parsedTime))
                {
                    errorMessage = "Wrong Time Format";
                    await context.EditResponseAsync(new DiscordWebhookBuilder().WithContent(errorMessage));
                    _logger.LogCallerInformation(context, errorMessage);

                    return;
                }

                if (parsedDate.Add(parsedTime) < DateTime.Now)
                {
                    errorMessage = "Date has to be in the future";
                    await context.EditResponseAsync(
                        new DiscordWebhookBuilder().WithContent(errorMessage));

                    _logger.LogCallerInformation(context, errorMessage);

                    return;
                }

                if (string.IsNullOrEmpty(eventName))
                {
                    errorMessage = "EventName is empty";
                    await context.EditResponseAsync(new DiscordWebhookBuilder().WithContent(errorMessage));
                    _logger.LogCallerInformation(context, errorMessage);

                    return;
                }

                if (eventChannel.Type != ChannelType.Text)
                {
                    errorMessage = "Channel is not a TextChannel";
                    await context.EditResponseAsync(
                        new DiscordWebhookBuilder().WithContent(errorMessage));
                    _logger.LogCallerInformation(context, errorMessage);

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
                        errorMessage = "Wrong Reminder Format";
                        await context.EditResponseAsync(
                            new DiscordWebhookBuilder().WithContent(errorMessage));
                        _logger.LogCallerInformation(context, errorMessage);

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
                _logger.Error(e, e.Message);
            }
        }
    }
}