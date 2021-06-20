using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscordBot.Modules
{
    public class HelpCommand : ModuleBase
    {
        [Command("help")]
        [Alias("hilfe")]
        public async Task Help()
        {
            string ret;

            const string version = "Version 3.0.0\n\n";

            const string uploadonly =
                "!uploadonly <ChannelID To Post to> : This Text send into this Channel will be Transfered to ChannelID Channel\n\n";

            const string clear = "!clear <amount to delete> : Deletes x messages in channel, " +
                                "deleting more than 6 Messages will get into RateLimit --> taking very long time due API rate limit\n\n";

            const string team =
                "!team <rolename To scramble> <amount of Teams> : Scrambles Member of Role into amount of Teams\n\n";

            const string createevent =
                "!event <name of event> <date of event dd.mm.yyyy> <time of event hh:mm> <channelId to post to> <Optional reminderarray '1d,2d,5m'>\n\n";

            const string events = "!events shows upcoming events\n\n";

            const string lsupload = "!lsupload shows upload only channels\n\n";

            const string split = "!split <group> <number of work to split>\n\n";

            const string delevent = "!delevent <id to delete>  deletes event\n\n";

            const string delupload = "!delupload <id to delete> deletes upload only channel\n\n";

            const string example = "!examples shows how to use the commands\n\n";

            ret = version + uploadonly + clear + team + createevent + events + lsupload + split + delevent + delupload + example;

            await ReplyAsync("", false, new EmbedBuilder() {Description = ret, Color = Color.Purple}.Build());
        }
    }
}