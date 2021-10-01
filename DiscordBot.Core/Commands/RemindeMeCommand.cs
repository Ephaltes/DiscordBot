using System.Threading.Tasks;
using DiscordBot.Core.Interfaces;
using DSharpPlus;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using Serilog;

namespace DiscordBot.Core.Commands
{
    public class RemindeMeCommand : ApplicationCommandModule
    {
        private readonly ILogger _logger;
        private readonly ITimeRepository _timeRepository;
        public RemindeMeCommand(ITimeRepository timeRepository, ILogger logger)
        {
            _logger = logger.ForContext(GetType());
            _timeRepository = timeRepository;
        }

        [SlashRequireUserPermissions(Permissions.Administrator)]
        [SlashCommand("RemindeMe", "Receive a DM for your Reminder")]
        public async Task RemindeMe(InteractionContext context,
            [Option("Name", "Name of the Event")] string reminderName,
            [Option("ReminderDate", "Date to Reminde")]
            string reminderDate,
            [Option("Time", "Time of the Event")] string reminderTime,
            [Option("ReOccurentIntervall", "reoccuring event all e.g. 1d,2d,2h")]
            string reOccuringIntervall = "")
        {
        }
    }
}