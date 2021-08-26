using System;
using System.Threading.Tasks;
using DiscordBot.Core.Entity;
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
            _logger = logger;
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
                await context.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                if (uploadChannel.Type != ChannelType.Text || postToChannel.Type != ChannelType.Text)
                {
                    await context.EditResponseAsync(
                        new DiscordWebhookBuilder().WithContent("Channel is not a TextChannel"));

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

                    await context.EditResponseAsync(new DiscordWebhookBuilder().WithContent("something went wrong"));
                    _logger.Error("Error adding upload only");
                }
                else
                {
                    await context.EditResponseAsync(new DiscordWebhookBuilder().WithContent("Channel already in List"));
                }
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                _logger.Error(e.StackTrace);
            }
        }
    }
}