using System;
using System.Net.Http;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DiscordBot.Extensions
{
    public static class AddServicesIServiceCollectionExtension
    {
        public static void AddServices(this IServiceCollection services, HostBuilderContext ctx)
        {
            services.AddLogging();

            services.AddSingleton<DiscordSocketClient>(new DiscordSocketClient(new DiscordSocketConfig()
                {AlwaysDownloadUsers = true}));
            services.AddSingleton<CommandService>();
            services.AddScoped<CommandHandlingService>();
            services.AddSingleton<HttpClient>();


            var sqlitePath = ctx.Configuration.GetSection("SqlitePath").Value;
            services.AddDbContext<DatabaseContext>(config => 
                { config.UseSqlite($"DataSource={sqlitePath}"); });
            services.AddTransient<IUploadOnlyRepository, UploadOnlyRepository>();
            services.AddTransient<IEventRepository, EventRepository>();
        }
    }
}