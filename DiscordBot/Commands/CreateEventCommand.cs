using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using DiscordBot.Database;
using DiscordBot.Entity;
using PowerArgs;

namespace DiscordBot.Modules
{
    public class CreateEventCommand : ModuleBase
    {
        private readonly IEventRepository _repository;

        public CreateEventCommand(IEventRepository repository)
        {
            _repository = repository;
        }

        [Command("event")]
        [Alias("reminder","remind","createevent","cevent","creminder")]
        public async Task CreateEvent(params string[] param)
        {
            try
            {
                EventArguments parsed = await Args.ParseAsync<EventArguments>(param);
                
                var entity = parsed.ToEntity();
                entity.ServerId = Context.Guild.Id;
                var channels = await Context.Guild.GetTextChannelsAsync();

                if (entity.Date < DateTime.Now)
                {
                    await ReplyAsync("Date has to be in the future");
                    return;
                }

                if (channels.FirstOrDefault(x => x.Id == entity.ChannelToPostId) == null)
                {
                    await ReplyAsync("Channel not found");
                    return;
                }

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