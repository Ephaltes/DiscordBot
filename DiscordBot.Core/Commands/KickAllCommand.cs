using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscordBot.Core.Attributes;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;

namespace DiscordBot.Core.Commands;

public class KickAll : ApplicationCommandModule
{
    [SlashCommand("Kickall", "Kick all except they have a specific role or excluded by name")]
    [RequireUserId(239024829188014081)]
    public async Task Kickall(InteractionContext context,
        [Option("excludeRole", "Role to exclude from Kick")]
        DiscordRole excludedRole = null)
    {
        await context.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
        await KickAllExcludeRole(excludedRole, context);
    }

    private async Task KickAllExcludeRole(DiscordRole? excludedRole, InteractionContext context)
    {
        IReadOnlyDictionary<ulong, DiscordMember>? members = context.Guild.Members;

        foreach (DiscordMember? member in members.Values)
        {
            if (member.IsBot || (excludedRole is not null && member.Roles.Contains(excludedRole)))
                continue;

            await member.RemoveAsync();
            Task.Delay(1000).Wait();
        }

        await context.EditResponseAsync(
            new DiscordWebhookBuilder().WithContent($"Kicked all users except ones in {excludedRole?.Name}"));
    }
}