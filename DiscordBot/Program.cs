using System;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;


namespace DiscordBot
{
    class Program
    {
        private DiscordSocketClient _client = null;
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    new Program().MainAsync().GetAwaiter().GetResult();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Task.Delay(5000).Wait();
                } 
            }
        }

        public async Task MainAsync()
        {
            // You should dispose a service provider created using ASP.NET
            // when you are finished using it, at the end of your app's lifetime.
            // If you use another dependency injection framework, you should inspect
            // its documentation for the best way to do this.
            using (var services = ConfigureServices())
            {
                _client = services.GetRequiredService<DiscordSocketClient>();

                _client.Log += LogAsync;
                _client.MessageReceived += Client_MessageReceived;
                services.GetRequiredService<CommandService>().Log += LogAsync;

                AppSettings settings = AppSettings.settings;

                // Tokens should be considered secret data and never hard-coded.
                // We can read from the environment variable to avoid hardcoding.
                await _client.LoginAsync(TokenType.Bot, settings.Token);
                await _client.StartAsync();

                // Here we initialize the logic required to register our commands.
                await services.GetRequiredService<CommandHandlingService>().InitializeAsync();
                bool load = SavedSettings.settings.Load();

                Console.WriteLine($"Loading savedSettings {load}");

                await Task.Delay(-1);
            }
        }

        private Task Client_MessageReceived(SocketMessage msg)
        {

            if (msg.Author.IsBot || !SavedSettings.settings.GetUploadOnlyList().ContainsKey(msg.Channel.Id) 
                                 || SavedSettings.settings.GetUploadOnlyList().ContainsKey(msg.Channel.Id) 
                                 && msg.Attachments.Count > 0)
                return Task.CompletedTask;
            
            var channel = msg.Channel as SocketGuildChannel;
            ulong channelToPostToId;
            SocketTextChannel channelToPost;
            
            SavedSettings.settings.GetUploadOnlyList().TryGetValue(msg.Channel.Id, out channelToPostToId);
            channelToPost = channel.Guild.TextChannels.FirstOrDefault(x => x.Id == channelToPostToId);

            if (channelToPost == null)
            {
                msg.Channel.SendMessageAsync("Channel To Post to doesn´t exist.");
                return Task.CompletedTask;
            }

            EmbedBuilder builder = new EmbedBuilder();

            builder.WithAuthor(msg.Author);
            builder.WithTimestamp(msg.Timestamp);
            builder.WithDescription(msg.Content);
            builder.Color = Color.Gold;

            channelToPost.SendMessageAsync("", false, builder.Build());
            msg.DeleteAsync();

            return Task.CompletedTask;
        }

        private Task LogAsync(LogMessage log)
        {

            switch (log.Severity)
            {
                case LogSeverity.Critical:
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogSeverity.Verbose:
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
            }

            Console.WriteLine($"{DateTime.Now,-19} [{log.Severity,8}] {log.Source}: {log.Message} {log.Exception}");
            Console.ResetColor();

            return Task.CompletedTask;
        }

        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<HttpClient>()
                .BuildServiceProvider();
        }
    }
}

