using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscordBot.Modules
{
    public class HelpCommand : ModuleBase
    {
        [Command("help")]
        [Alias("hilfe")]
        public async Task Help(params string[] param)
        {
            string ret;

            string version = "Version 3.0.0\n";
            
            string uploadonly = "!uploadonly <ChannelID To Post to> : This Text send into this Channel will be Transfered to ChannelID Channel\n\n";
            
            string clear = "!clear <amount to delete> : Deletes x messages in channel, " +
                           "deleting more than 6 Messages will get into RateLimit --> taking very long time due API rate limit\n\n";
            
            string team = "!team <rolename To scramble> <amount of Teams> : Scrambles Member of Role into amount of Teams\n\n";


            ret = version + uploadonly+clear+team;
            
            await ReplyAsync("",false,new EmbedBuilder(){Description = ret,Color = Color.Purple}.Build());
        }
    }
}