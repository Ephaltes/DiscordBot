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
    public class ListEventCommand : ApplicationCommandModule
    {
        private readonly ILogger _logger;
        private readonly IEventRepository _repository;

        public ListEventCommand(IEventRepository repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger.ForContext(GetType());
        }

        [SlashRequireUserPermissions(Permissions.Administrator)]
        [SlashCommand("listEvents", "Shows upcoming events")]
        public async Task ListEvents(InteractionContext context)
        {
            try
            {
                _logger.LogCallerInformation(context);
                List<EventEntity> list = await _repository.GetAll();

                if (list.Count < 1)
                {
                    await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                        new DiscordInteractionResponseBuilder().AddEmbed(
                            EmbeddedHelper.CreateDefaultEmbed("No Events")));

                    return;
                }

                string ret = "";

                IReadOnlyList<DiscordChannel> channels = await context.Guild.GetChannelsAsync();

                foreach (var entity in list)
                {
                    string reminder = "";
                    foreach (var timeEntity in entity.TimeEntities)
                        reminder += $"{timeEntity.Time}\n";

                    ret += $"ID: {entity.Id}\n" +
                           $"Name: {entity.Name}\n" +
                           $"Date: {entity.Date.ToShortDateString()}\n" +
                           $"Time: {entity.Date.TimeOfDay}\n" +
                           $"Reminder:\n{reminder}" +
                           $"PostTo: {entity.ChannelToPostId} ({channels.FirstOrDefault(x => x.Id == entity.ChannelToPostId)?.Name})\n\n";
                }

                await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().AddEmbed(
                        EmbeddedHelper.CreateDefaultEmbed(ret)));
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }
        }
    }
}