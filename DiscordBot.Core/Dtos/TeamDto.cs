using System;
using DSharpPlus.Entities;

namespace DiscordBot.Core.Dtos
{
    public class TeamDto
    {
        public DiscordRole Role { get; init; }
        public int? TeamAmount { get; init; }

        public TeamDto(DiscordRole role, int teamAmount)
        {
            Role = role;
            TeamAmount = teamAmount;
        }

        public TeamDto(DiscordRole role, long teamAmount)
        {
            Role = role;
            try
            {
                TeamAmount = Convert.ToInt32(teamAmount);
            }
            catch (Exception)
            {
                TeamAmount = null;
            }
        }
    }
}