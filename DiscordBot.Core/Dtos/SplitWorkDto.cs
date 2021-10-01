using System;
using DSharpPlus.Entities;

namespace DiscordBot.Core.Dtos
{
    public class SplitWorkDto
    {
        public DiscordRole Role { get; init; }
        public int? TotalWorkTasks { get; init; }

        public SplitWorkDto(DiscordRole role, int totalWorkTasks)
        {
            Role = role;
            TotalWorkTasks = totalWorkTasks;
        }

        public SplitWorkDto(DiscordRole role, long totalWorkTasks)
        {
            Role = role;
            try
            {
                TotalWorkTasks = Convert.ToInt32(totalWorkTasks);
            }
            catch (Exception e)
            {
                TotalWorkTasks = null;
            }
        }
    }
}