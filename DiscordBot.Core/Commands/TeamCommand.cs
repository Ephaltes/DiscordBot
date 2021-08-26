using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscordBot.Core.Extension;
using DiscordBot.Core.Helper;
using DSharpPlus;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using Serilog;

namespace DiscordBot.Core.Commands
{
    public class TeamCommand : ApplicationCommandModule
    {
        private readonly ILogger _logger;
        public TeamCommand(ILogger logger)
        {
            _logger = logger.ForContext(GetType());
        }

        [RequireGuild]
        [SlashRequirePermissions(Permissions.Administrator)]
        [SlashCommand("Team", "Scrambles the User into Teams")]
        public async Task Team(InteractionContext context,
            [Option("role", "Role to scramble")] DiscordRole role,
            [Option("teamAmount", "Number of Teams")]
            long teamAmount)
        {
            try
            {
                _logger.LogCallerInformation(context);

                if (teamAmount < 1)
                    await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                        new DiscordInteractionResponseBuilder().WithContent("Team size < 1"));

                IEnumerable<DiscordMember> members = context.Guild.Members.Values;

                List<DiscordMember> memberInRole = members
                    .Where(member => member.Roles.Any(memberRole => memberRole == role))
                    .OrderBy(_ => Guid.NewGuid()).ToList();


                string[] message = new string[teamAmount];

                for (int i = 0; i < memberInRole.Count; i++)
                {
                    int team = i % (int)teamAmount;
                    message[team] += $"{memberInRole[i].Username}\n";
                }

                string ret = "";

                for (int i = 0; i < teamAmount; i++)
                    if (!string.IsNullOrWhiteSpace(message[i]))
                        ret += $"\nTeam {i + 1}: \n" + message[i];

                DiscordMessageBuilder builder = new DiscordMessageBuilder
                {
                    Embed = EmbeddedHelper.CreateDefaultEmbed(ret)
                };

                await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder(builder));
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }
        }
    }
}