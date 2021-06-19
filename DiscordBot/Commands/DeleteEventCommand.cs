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
        [Alias("removeevent","devent","revent")]
        public async Task DeleteEvent(params string[] param)
        {
            try
            {
                if (param.Length < 1)
                {
                    await ReplyAsync("wrong Parameter");
                    return;
                }
                
                var eventid = new Guid(param[0]);

                if (await _repository.Delete(eventid))
                {
                    await ReplyAsync($"Event deleted successful");
                    return;
                }

                await ReplyAsync("Error occurred");
            }
            catch (ArgException e)
            {
                await ReplyAsync(e.Message);
            }
        }
    }
}