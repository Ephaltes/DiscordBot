using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using DiscordBot.Database;
using DiscordBot.Entity;
using PowerArgs;

namespace DiscordBot.Modules
{
    public class DeleteEventCommand : ModuleBase
    {
        private readonly IEventRepository _repository;

        public DeleteEventCommand(IEventRepository repository)
        {
            _repository = repository;
        }

        [Command("deleteevent")]
        [Alias("removeevent")]
        public async Task CreateEvent(params string[] param)
        {
            try
            {
                EventArguments parsed = await Args.ParseAsync<EventArguments>(param);

                var entity = parsed.ToEntity();

                await _repository.Add(entity);

                await ReplyAsync($"Event '{entity.Name}' added successful");
            }
            catch (ArgException e)
            {
                await ReplyAsync(e.Message);
            }
        }
    }
}