using System;
using System.Threading.Tasks;
using Discord;

namespace DiscordBot
{
    public class LoggingService
    {
        public static Task Log(LogMessage log)
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
    }
}