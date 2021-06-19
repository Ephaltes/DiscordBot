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
    public class ListEventCommand : ModuleBase
    {
        private readonly IEventRepository _repository;

        public ListEventCommand(IEventRepository repository)
        {
            _repository = repository;
        }

        [Command("listevent")]
        [Alias("levent","events")]
        public async Task ListEvents()
        {
            try
            {
                var list = await _repository.GetAll();

                if (list.Count < 1)
                {
                    await ReplyAsync("",false,new EmbedBuilder(){Description = $"No Events",Color = Color.Purple}.Build());
                    return;
                }
                string ret = "";
                
                var channels = await Context.Guild.GetTextChannelsAsync();

                foreach (var entity in list)
                {
                    ret += $"ID: {entity.Id}\n" +
                           $"Name: {entity.Name}\n" +
                           $"Date: {entity.Date.ToShortDateString()}\n" +
                           $"Time: {entity.Date.TimeOfDay}\n" +
                           $"PostTo: {entity.ChannelToPostId} ({channels.FirstOrDefault(x=>x.Id==entity.ChannelToPostId)?.Name})\n\n";
                }
                
                await ReplyAsync("",false,new EmbedBuilder(){Description = ret,Color = Color.Purple}.Build());
            }
            catch (ArgException e)
            {
                await ReplyAsync(e.Message);
            }
        }
    }
}