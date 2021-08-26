using System;
using System.Threading.Tasks;
using DiscordBot.Core.Extension;
using DiscordBot.Core.Interfaces;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using Serilog;

namespace DiscordBot.Core.Commands
{
    public class DeleteEventCommand : ApplicationCommandModule
    {
        private readonly ILogger _logger;
        private readonly IEventRepository _repository;

        public DeleteEventCommand(IEventRepository repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger.ForContext(GetType());
        }

        [SlashRequireUserPermissions(Permissions.Administrator)]
        [SlashCommand("DeleteEvent", "Deletes an Event by Id")]
        public async Task DeleteEvent(InteractionContext context,
            [Option("EventId", "Id of the Event to delete")]
            string eventId)
        {
            try
            {
                _logger.LogCallerInformation(context);
                Guid eventid = new Guid(eventId);

                if (await _repository.Delete(eventid))
                {
                    await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                        new DiscordInteractionResponseBuilder().WithContent("Event deleted successful"));

                    return;
                }

                const string errorMessage = "something went wrong";
                await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().WithContent(errorMessage));
                _logger.LogCallerInformation(context, errorMessage);
            }
            catch (Exception e)
            {
                _logger.Error(e.StackTrace);
            }
        }
    }
}