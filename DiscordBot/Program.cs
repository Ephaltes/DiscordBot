using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TimeSpanParserUtil;


namespace DiscordBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            CultureInfo.CurrentCulture = new CultureInfo("de-AT", false);
            CultureInfo.CurrentUICulture = new CultureInfo( "de-AT", false );
            Console.WriteLine("CurrentCulture is {0}.", CultureInfo.CurrentCulture.Name);
            Console.WriteLine("CurrentUICulture is {0}.", CultureInfo.CurrentUICulture.Name);
            
            using IHost host = CreateHostBuilder(args).Build();

            IServiceProvider services = host.Services;
            
            var configuration = services.GetRequiredService<IConfiguration>();
            DiscordSocketClient client = services.GetRequiredService<DiscordSocketClient>();

            client.Log += LoggingService.Log;
            services.GetRequiredService<CommandService>().Log += LoggingService.Log;

            string apiToken = configuration.GetSection("ApiToken").Value;

            // Tokens should be considered secret data and never hard-coded.
            // We can read from the environment variable to avoid hardcoding.
            await client.LoginAsync(TokenType.Bot, apiToken);
            await client.StartAsync();

            // Here we initialize the logic required to register our commands.
            await services.GetRequiredService<CommandHandlingService>().InitializeAsync();

            await host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    
                    var appsettingsPath = "appsettings.json";

                    //var t = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER");

                    var pathFromEnv = Environment.GetEnvironmentVariable("AppSettingsPath");

                    if (!string.IsNullOrEmpty(pathFromEnv))
                        appsettingsPath = pathFromEnv;

                    
                    configApp.AddJsonFile(appsettingsPath, optional: false);
                    configApp.AddCommandLine(args);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddServices(hostContext);
                });
    }
}