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
    public class DeleteUploadOnlyCommand : ApplicationCommandModule
    {
        private readonly ILogger _logger;
        private readonly IUploadOnlyRepository _repository;

        public DeleteUploadOnlyCommand(IUploadOnlyRepository repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [SlashRequireUserPermissions(Permissions.Administrator)]
        [SlashCommand("DeleteUploadOnlyChannel", "Upload Only Channel reverted back to normal")]
        public async Task DeleteEvent(InteractionContext context,
            [Option("UploadOnlyChannel", "Channel where you can only Upload")]
            DiscordChannel uploadChannel)
        {
            try
            {
                await context.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                UploadOnlyEntity? entity = await _repository.GetByChannelId(uploadChannel.Id);

                if (entity != null && await _repository.Delete(entity.Id))
                {
                    await context.EditResponseAsync(
                        new DiscordWebhookBuilder().WithContent("Uploadonly Channel deleted successful"));

                    return;
                }

                await context.EditResponseAsync(
                    new DiscordWebhookBuilder().WithContent("Error occured"));
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                _logger.Error(e.StackTrace);
            }
        }
    }
}