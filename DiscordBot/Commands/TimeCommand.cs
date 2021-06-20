using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscordBot.Modules
{
    public class TimeCommand : ModuleBase
    {
        [Command("time")]
        public async Task Time(params string[] amount)
        {
            await ReplyAsync(DateTime.Now.ToLocalTime().ToString());
        }
    }
}