using System;
using System.Collections.Generic;
using DSharpPlus.Entities;
using TimeSpanParserUtil;

namespace DiscordBot.Core.Dtos
{
    public record CreateEventDtoss(string Name, string Date, string Time, DiscordChannel EventChannel,
        string Reminder);

    public class CreateEventDto
    {
        private static readonly char[] _delimiter = { ' ', ',', '.' };
        public string Name { get; init; }
        public DateOnly? Date { get; init; }

        public TimeOnly? Time { get; init; }

        public DiscordChannel EventChannel { get; init; }

        public List<TimeSpan>? Reminder { get; init; } = new List<TimeSpan>();

        public CreateEventDto(string name, DateOnly date, TimeOnly time, DiscordChannel eventChannel,
            List<TimeSpan> reminder)
        {
            Name = name;
            Date = date;
            Time = time;
            EventChannel = eventChannel;
            Reminder = reminder;
        }
        public CreateEventDto(string name, string date, string time, DiscordChannel eventChannel, string reminderList)
        {
            Name = name;

            bool dateSuccess = DateOnly.TryParse(date, out DateOnly parsedDate);

            if (dateSuccess)
                Date = parsedDate;

            bool timeSuccess = TimeOnly.TryParse(time, out TimeOnly parsedTime);

            if (timeSuccess)
                Time = parsedTime;

            EventChannel = eventChannel;

            string[] reminderSplitted = reminderList.Split(_delimiter,
                StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

            foreach (string reminder in reminderSplitted)
            {
                if (!TimeSpanParser.TryParse(reminder, out TimeSpan parsedReminderTime))
                {
                    Reminder = null;

                    return;
                }

                Reminder?.Add(parsedReminderTime);
            }
        }
    }
}