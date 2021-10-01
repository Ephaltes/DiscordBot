using System;
using DSharpPlus.Entities;
using TimeSpanParserUtil;

namespace DiscordBot.Core.Dtos
{
    public record RemindeMeDto(string Name, string Date, string Time, string ReOccuringIntervall);

    public class ReminderMeDto
    {
        public string Name { get; init; }
        public DateOnly? Date { get; init; }
        public TimeOnly? Time { get; init; }
        public TimeSpan? ReoccuringIntervall { get; init; }

        public DiscordUser User { get; init; }

        public ReminderMeDto(string name, DateOnly date, TimeOnly time, TimeSpan reoccuringIntervall, DiscordUser user)
        {
            Name = name;
            Date = date;
            Time = time;
            ReoccuringIntervall = reoccuringIntervall;
            User = user;
        }

        public ReminderMeDto(string name, string date, string time, string reoccuringIntervall, DiscordUser user)
        {
            Name = name;
            User = user;

            bool dateSuccess = DateOnly.TryParse(date, out DateOnly parsedDate);

            if (dateSuccess)
                Date = parsedDate;

            bool timeSuccess = TimeOnly.TryParse(time, out TimeOnly parsedTime);

            if (timeSuccess)
                Time = parsedTime;

            if (TimeSpanParser.TryParse(reoccuringIntervall, out TimeSpan parsedIntervall))
                ReoccuringIntervall = parsedIntervall;
        }
    }
}