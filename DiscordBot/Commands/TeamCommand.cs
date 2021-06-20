using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Serilog;

namespace DiscordBot.Modules
{
    public class TeamCommand : ModuleBase
    {
        private readonly ILogger _logger = Log.ForContext<TeamCommand>();
        
        [RequireUserPermission(GuildPermission.Administrator)]
        [Command("team")]
        [Alias("scramble")]
        public async Task Team(params string[] param)
        {
            var channel = Context.Channel as SocketGuildChannel;
            SocketRole socketRole;
            int teamCount;
            
            try
            {
                if (param.Length < 2 
                    || (socketRole = channel.Guild.Roles
                        .FirstOrDefault(x => x.Name.ToLower() == param[0].ToLower())) == null
                    || (teamCount = Convert.ToInt32(param[1])) < 1 ) // cant find Group
                {
                    await Context.Channel.SendMessageAsync("Wrong arguments");
                    return;
                }
            }
            catch (Exception e)
            {
                _logger.Error(e,e.Message);
                await Context.Channel.SendMessageAsync("Wrong arguments");
                return;
            }
            
            var memberList = socketRole.Members.OrderBy(x => Guid.NewGuid()).ToList();

            string[] message = new string[teamCount];
            
            for (int i = 0; i < memberList.Count; i++)
            {
                int team = i % teamCount;
                message[team] += $"{memberList[i].Username}\n";
            }

            string ret = "";

            for (int i = 0; i < teamCount; i++)
            {
                if (!string.IsNullOrWhiteSpace(message[i]))
                {
                    ret += $"\nTeam {i + 1}: \n" + message[i];
                }
            }
            
            await Context.Channel.SendMessageAsync(null, false, new EmbedBuilder() {Color=Color.Blue,Description = ret}.Build());
        }
    }
}