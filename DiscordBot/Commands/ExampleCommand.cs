using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscordBot.Modules
{
    public class ExampleCommand : ModuleBase
    {
        [Command("examples")]
        [Alias("example")]
        public async Task Example()
        {
            string ret;

            const string uploadonly = "!uploadonly 4321541\n\n";

            const string clear = "!clear 10 \n\n";

            const string team = "!team og 2\n\n";

            const string createevent = "!event morning 30.12.2023 08:30 43231 1d,2d\n\n";

            const string events = "!events\n\n";

            const string lsupload = "!lsupload\n\n";

            const string split = "!split og 41\n\n";

            const string delevent = "!delevent 5\n\n";

            const string delupload = "!delupload 10\n\n";

            const string example = "!examples\n\n";

            ret = uploadonly + clear + team + createevent + events + lsupload + split + delevent + delupload +
                  example;

            await ReplyAsync("", false, new EmbedBuilder() {Description = ret, Color = Color.Purple}.Build());
        }
    }
}