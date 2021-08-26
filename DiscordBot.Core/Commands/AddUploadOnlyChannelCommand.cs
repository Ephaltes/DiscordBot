using System;
using System.Threading.Tasks;
using DiscordBot.Core.Entity;
using DiscordBot.Core.Extension;
using DiscordBot.Core.Interfaces;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using Serilog;

namespace DiscordBot.Core.Commands
{
    public class AddUploadOnlyChannelCommand : ApplicationCommandModule
    {
        private readonly ILogger _logger;
        private readonly IUploadOnlyRepository _repository;

        public AddUploadOnlyChannelCommand(IUploadOnlyRepository repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger.ForContext(GetType());
        }


        [SlashRequireUserPermissions(Permissions.Administrator)]
        [SlashCommand("AddUploadOnlyChannel", "Channel can only be used for Uploads")]
        public async Task AddUploadOnlyChannel(InteractionContext context,
            [Option("UploadOnlyChannel", "Channel where you can only Upload")]
            DiscordChannel uploadChannel,
            [Option("PostToChannel", "Channel to Post the content from UploadChannel if it is not an Upload Content")]
            DiscordChannel postToChannel)
        {
            try
            {
                string errorMessage = "";

                await context.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
                _logger.LogCallerInformation(context);

                if (uploadChannel.Type != ChannelType.Text || postToChannel.Type != ChannelType.Text)
                {
                    errorMessage = "Channel is not a TextChannel";
                    await context.EditResponseAsync(
                        new DiscordWebhookBuilder().WithContent(errorMessage));

                    _logger.LogCallerInformation(context, errorMessage);

                    return;
                }

                ulong uploadChannelId = uploadChannel.Id;
                ulong serverid = context.Guild.Id;
                ulong postToChannelId = postToChannel.Id;

                if (await _repository.GetByChannelId(uploadChannelId) == null)
                {
                    UploadOnlyEntity entity = new UploadOnlyEntity
                        { ChannelId = uploadChannelId, ChannelToPostId = postToChannelId, ServerId = serverid };

                    bool result = await _repository.Add(entity);

                    if (result)
                    {
                        await context.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Channel added"));

                        return;
                    }

                    errorMessage = "something went wrong";
                    await context.EditResponseAsync(new DiscordWebhookBuilder().WithContent(errorMessage));
                    _logger.LogCallerInformation(context, errorMessage);
                }
                else
                {
                    errorMessage = "Channel already in List";
                    await context.EditResponseAsync(new DiscordWebhookBuilder().WithContent(errorMessage));
                    _logger.LogCallerInformation(context, errorMessage);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }
        }
    }
}