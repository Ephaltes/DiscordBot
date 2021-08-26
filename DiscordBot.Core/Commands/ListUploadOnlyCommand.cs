using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DiscordBot.Core.Entity;
using DiscordBot.Core.Extension;
using DiscordBot.Core.Helper;
using DiscordBot.Core.Interfaces;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using Serilog;

namespace DiscordBot.Core.Commands
{
    public class ListUploadOnlyCommand : ApplicationCommandModule
    {
        private readonly ILogger _logger;
        private readonly IUploadOnlyRepository _repository;

        public ListUploadOnlyCommand(IUploadOnlyRepository repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger.ForContext(GetType());
        }

        [SlashRequireUserPermissions(Permissions.Administrator)]
        [SlashCommand("ListUploadOnlyChannel", "Shows Upload Only Channels")]
        public async Task ListUploadOnly(InteractionContext context)
        {
            try
            {
                _logger.LogCallerInformation(context);
                await context.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                List<UploadOnlyEntity> list = await _repository.GetAllbyServerId(context.Guild.Id);

                if (list.Count < 1)
                {
                    await context.EditResponseAsync(
                        new DiscordWebhookBuilder().AddEmbed(EmbeddedHelper.CreateDefaultEmbed("No Channels")));

                    return;
                }

                string ret = "";

                IReadOnlyList<DiscordChannel>? channels = await context.Guild.GetChannelsAsync();

                foreach (var entity in list)
                    ret += $"ID: {entity.Id}\n" +
                           $"UploadOnlyChannelID: {entity.ChannelId} ({channels.FirstOrDefault(x => x.Id == entity.ChannelId)?.Name})\n" +
                           $"ChannelToPostToID: {entity.ChannelToPostId} ({channels.FirstOrDefault(x => x.Id == entity.ChannelToPostId)?.Name})\n\n";

                await context.EditResponseAsync(
                    new DiscordWebhookBuilder().AddEmbed(EmbeddedHelper.CreateDefaultEmbed(ret)));
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }
        }
    }
}