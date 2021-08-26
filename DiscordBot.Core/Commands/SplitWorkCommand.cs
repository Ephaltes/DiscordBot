using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscordBot.Core.Helper;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using Serilog;

namespace DiscordBot.Core.Commands
{
    public class SplitWorkCommand : ApplicationCommandModule
    {
        private readonly ILogger _logger;
        public SplitWorkCommand(ILogger logger)
        {
            _logger = logger;
        }


        [SlashRequireUserPermissions(Permissions.Administrator)]
        [SlashCommand("SplitWork", "splits workTask equally between memberRoles ")]
        public async Task SplitWork(InteractionContext context,
            [Option("role", "Role to divide")] DiscordRole role,
            [Option("totalWorkTasks", "Total amount of WorkTasks")]
            long totalWorkTasks)
        {
            try
            {
                IEnumerable<DiscordMember> members = context.Guild.Members.Values;

                List<DiscordMember> memberInRole = members
                    .Where(member => member.Roles.Any(memberRole => memberRole == role))
                    .OrderBy(_ => Guid.NewGuid()).ToList();

                List<int> workList = Enumerable.Range(1, Convert.ToInt32(totalWorkTasks)).OrderBy(_ => Guid.NewGuid())
                    .ToList();

                List<List<int>> workForEachMember = new List<List<int>>();
                memberInRole.ForEach(_ => workForEachMember.Add(new List<int>()));

                for (int i = 0; i < workList.Count; i++)
                    workForEachMember[i % memberInRole.Count].Add(workList[i]);

                string ret = "";

                foreach (DiscordMember member in memberInRole.OrderBy(_ => Guid.NewGuid()))
                {
                    ret += $"{member.Username}: \n";
                    ret += $" {string.Join(", ", workForEachMember[0].OrderBy(a => a))} \n\n";
                    workForEachMember.RemoveAt(0);
                }

                await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder(new DiscordMessageBuilder
                        { Embed = EmbeddedHelper.CreateDefaultEmbed(ret) }));
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                _logger.Error(e.StackTrace);
            }
        }
    }
}