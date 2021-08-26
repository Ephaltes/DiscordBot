using System;
using System.Globalization;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace DiscordBot.Core.Commands
{
    public class TimeCommand : ApplicationCommandModule
    {
        [SlashCommand("Time", "Displays current Time")]
        public async Task Time(InteractionContext context)
        {
            await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                new DiscordInteractionResponseBuilder().WithContent(DateTime.Now.ToLocalTime()
                    .ToString(CultureInfo.CurrentCulture)));
        }
    }
}