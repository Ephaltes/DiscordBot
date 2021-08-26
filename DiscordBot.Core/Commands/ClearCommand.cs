using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using Serilog;

namespace DiscordBot.Core.Commands
{
    public class ClearCommand : ApplicationCommandModule
    {
        private readonly ILogger _logger;

        public ClearCommand(ILogger logger)
        {
            _logger = logger;
        }

        [RequireGuild]
        [SlashRequirePermissions(Permissions.Administrator)]
        [SlashCommand("Clear", "Deletes x Messages")]
        public async Task Clear(InteractionContext context,
            [Option("amount", "amount to delete")] long amount)
        {
            if (amount < 1)
            {
                await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource,
                    new DiscordInteractionResponseBuilder().WithContent("wrong Parameter"));

                return;
            }

            _ = Task.Run(async () =>
                await DeleteTask(amount, context)); //start in other thread and let it run in background till finish

            await context.CreateResponseAsync(InteractionResponseType.DeferredChannelMessageWithSource);
        }

        private async Task DeleteTask(long amount, InteractionContext context)
        {
            try
            {
                const double limit = 100.0;

                var rounds = Math.Ceiling(amount / limit);

                for (var i = 0; i < rounds; i++)
                {
                    int tempAmount;

                    if (amount > 100)
                    {
                        amount -= 100;
                        tempAmount = 100;
                    }
                    else
                    {
                        tempAmount = Convert.ToInt32(amount);
                    }

                    IReadOnlyList<DiscordMessage> messages = await context.Channel.GetMessagesAsync(tempAmount);

                    foreach (DiscordMessage message in messages)
                    {
                        await message.DeleteAsync();
                        Task.Delay(1000).Wait();
                    }

                    await context.EditResponseAsync(
                        new DiscordWebhookBuilder().WithContent($"Finished deleting {amount} Messages"));
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
            }
        }
    }
}