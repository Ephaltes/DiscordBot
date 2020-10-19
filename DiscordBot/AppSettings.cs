using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DiscordBot
{
    public class AppSettings
    {

        private static AppSettings _settings = null;

        private static object lockobject = new object();

        public string Token { get; set; }

        public static AppSettings settings
        {
            //Prüft ob es null ist wenn null dann leg für _settings eine neue AppSettings an
            get
            {
                lock (lockobject)
                {
                    return _settings ??= new AppSettings();
                }
            }
        }

        private AppSettings()
        {
            var configurationBuilder = new ConfigurationBuilder();
            //var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile("appsettings.json", false, true);

            var root = configurationBuilder.Build();
            var appSetting = root.GetSection("Bot");
            Token = appSetting["Token"];
        }
    }
}
