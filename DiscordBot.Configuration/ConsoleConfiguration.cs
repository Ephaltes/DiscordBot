using DiscordBot.Configuration.Module;
using DiscordBot.Core.Handler;
using DiscordBot.Core.Interfaces;
using Discordbot.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordBot.Configuration
{
    public class ConsoleConfiguration
    {
        public ServiceProvider Setup()
        {
            IConfigurationRoot configurationRoot = AppSettingsModule.GetAppConfig();
            ServiceCollection services = new ServiceCollection();

            services.AddLogModule(configurationRoot);
            services.AddSingleton(configurationRoot);

            string sqlitePath = configurationRoot.GetSection("SqlitePath").Value;

            services.AddDbContext<DatabaseContext>(config =>
                {
                    config.UseLazyLoadingProxies();
                    config.UseSqlite($"DataSource={sqlitePath}");
                },
                ServiceLifetime.Transient);

            services.AddTransient<IUploadOnlyRepository, UploadOnlyRepository>();
            services.AddTransient<IEventRepository, EventRepository>();
            services.AddTransient<ITimeRepository, TimeRepository>();
            services.AddTransient<IMessageHandler, MessageHandler>();
            services.AddTransient<IDiscordEventHandler, DiscordEventHandler>();

            services.AddSingleton<Core.DiscordBot>();

            return services.BuildServiceProvider();
        }
    }
}