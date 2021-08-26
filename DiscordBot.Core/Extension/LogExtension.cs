using DSharpPlus.SlashCommands;
using Serilog;

namespace DiscordBot.Core.Extension
{
    public static class LogExtension
    {
        public static void LogCallerInformation(this ILogger logger, InteractionContext context)
        {
            string guildName = context.Guild.Name;
            string userName = context.Member.Username;
            string command = context.CommandName;

            logger.Information($"ServerName: '{guildName}'  User: '{userName}'  called '{command}'");
        }

        public static void LogCallerInformation(this ILogger logger, InteractionContext context, string errorMessage)
        {
            string guildName = context.Guild.Name;
            string userName = context.Member.Username;
            string command = context.CommandName;

            logger.Information($"ServerName: '{guildName}'  User: '{userName}'  called '{command}'  " +
                               $"with Error: '{errorMessage}'");
        }
    }
}