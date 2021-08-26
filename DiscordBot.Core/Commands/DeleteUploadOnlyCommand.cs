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
    public class DeleteUploadOnlyCommand : ApplicationCommandModule
    {
        private readonly ILogger _logger;
        private readonly IUploadOnlyRepository _repository;

        public DeleteUploadOnlyCommand(IUploadOnlyRepository repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger.ForContext(GetType());
        }

        [SlashRequireUserPermissions(Permissions.Administrator)]
        [SlashCommand("DeleteUploadOnlyChannel", "Upload Only Channel reverted back to normal")]
        public async Task DeleteEvent(InteractionContext context,
            [Option("UploadOnlyChannel", "Channel where you can only Upload")]
            DiscordChannel uploadChannel)
        {
            try
            {
                _logger.LogCallerInformation(context);
                await context.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);

                UploadOnlyEntity? entity = await _repository.GetByChannelId(uploadChannel.Id);

                if (entity != null && await _repository.Delete(entity.Id))
                {
                    await context.EditResponseAsync(
                        new DiscordWebhookBuilder().WithContent("Uploadonly Channel deleted successful"));

                    return;
                }

                const string errorMessage = "something went wrong";
                await context.EditResponseAsync(
                    new DiscordWebhookBuilder().WithContent(errorMessage));
                _logger.LogCallerInformation(context, errorMessage);
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }
        }
    }
}