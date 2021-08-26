using DiscordBot.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordBot.ConsoleApp
{
    internal class Program
    {
        private static void Main()
        {
            ServiceProvider container = new ConsoleConfiguration().Setup();

            Core.DiscordBot bot = container.GetRequiredService<Core.DiscordBot>();
            bot.Start().GetAwaiter().GetResult();
        }
    }
}