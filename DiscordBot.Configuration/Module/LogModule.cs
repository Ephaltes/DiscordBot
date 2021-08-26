using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace DiscordBot.Configuration.Module
{
    public static class LogModule
    {
        public static IServiceCollection AddLogModule(this IServiceCollection services, IConfiguration config)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .Enrich.FromLogContext()
                .CreateLogger();

            services.AddSingleton(Log.Logger);

            return services;
        }
    }
}