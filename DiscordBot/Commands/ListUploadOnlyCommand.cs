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
    public class ListUploadOnlyCommand : ModuleBase
    {
        private readonly IUploadOnlyRepository _repository;

        public ListUploadOnlyCommand(IUploadOnlyRepository repository)
        {
            _repository = repository;
        }

        [Command("listuploadonly")]
        [Alias("listupload","lupload")]
        public async Task ListUploadOnly()
        {
            try
            {
                var list = await _repository.GetAllbyServerId(Context.Guild.Id);

                string ret = "";

                var channels = await Context.Guild.GetTextChannelsAsync();
                
                foreach (var entity in list)
                {
                    ret += $"ID: {entity.Id}\n" +
                           $"UploadOnlyChannelID: {entity.ChannelId}\n ({channels.FirstOrDefault(x=>x.Id==entity.ChannelId)?.Name})" +
                           $"ChannelToPostToID: {entity.ChannelToPostId} ({channels.FirstOrDefault(x=>x.Id==entity.ChannelToPostId)?.Name})\n\n";
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