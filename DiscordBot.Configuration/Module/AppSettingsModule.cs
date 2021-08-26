using System;
using Microsoft.Extensions.Configuration;

namespace DiscordBot.Configuration.Module
{
    public class AppSettingsModule
    {
        public static IConfigurationRoot GetAppConfig()
        {
            string appSettingsPath = "appsettings.json";

            string? pathFromEnvironment = Environment.GetEnvironmentVariable("AppSettingsPath");
            if (!string.IsNullOrEmpty(pathFromEnvironment))
                appSettingsPath = pathFromEnvironment;

            ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddJsonFile(appSettingsPath, false);

            return configurationBuilder.Build();
        }
    }
}