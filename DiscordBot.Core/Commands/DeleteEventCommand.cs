using System;
using System.Threading.Tasks;
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
            _logger = logger;
        }

        [SlashRequireUserPermissions(Permissions.Administrator)]
        [SlashCommand("DeleteEvent", "Deletes an Event by Id")]
        public async Task DeleteEvent(InteractionContext context,
            [Option("EventId", "Id of the Event to delete")]
            string eventId)
        {
            try
            {
                Guid eventid = new Guid(eventId);

                if (await _repository.Delete(eventid))
                {
                    await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                        new DiscordInteractionResponseBuilder().WithContent("Event deleted successful"));

                    return;
                }

                await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().WithContent("Error occurred"));
            }
            catch (Exception e)
            {
                _logger.Error(e.Message);
                _logger.Error(e.StackTrace);
            }
        }
    }
}