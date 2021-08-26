using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordBot.Core.Handler;
using DiscordBot.Core.Helper;
using DiscordBot.Core.Interfaces;
using DSharpPlus;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace DiscordBot.Core
{
    public class DiscordBot
    {
        private readonly IConfigurationRoot _configuration;
        private readonly IMessageHandler _messageHandler;
        private readonly IServiceProvider _services;
        public DiscordBot(IConfigurationRoot configuration, IServiceProvider services,
            IMessageHandler messageHandler)
        {
            _configuration = configuration;
            _services = services;
            _messageHandler = messageHandler;
        }
        public async Task Start()
        {
            DiscordConfiguration discordConfig = new DiscordConfiguration
            {
                Token = _configuration.GetSection("ApiToken").Value,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.All
            };

            DiscordClient client = new DiscordClient(discordConfig);
            IDiscordEventHandler discordEventHandler = new DiscordEventHandler(
                _services.GetRequiredService<IEventRepository>(),
                _services.GetRequiredService<ILogger>(),
                client);

            SlashCommandsExtension slash = client.UseSlashCommands(new SlashCommandsConfiguration
            {
                Services = _services
            });

            IEnumerable<Type> slashCommandList =
                ReflectionHelper.GetClassesFromBaseClass<ApplicationCommandModule>();

            foreach (Type type in slashCommandList)
            {
                slash.RegisterCommands(type, 444956433633640468);
            }

            client.MessageCreated += _messageHandler.MessageReceived;

            _ = Task.Run(async () => await discordEventHandler.StartPolling());

            await client.ConnectAsync();
            await Task.Delay(-1);
        }
    }
}