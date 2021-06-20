using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Serilog;

namespace DiscordBot.Modules
{
    public class SplitWorkCommand : ModuleBase
    {
        private readonly ILogger _logger = Log.ForContext<SplitWorkCommand>();
        
        [RequireUserPermission(GuildPermission.Administrator)]
        [Command("splitwork")]
        [Alias("split")]
        public async Task SplitWork(params string[] param)
        {
            var channel = Context.Channel as SocketGuildChannel;
            SocketRole socketRole;
            int maxWorkNum;

            try
            {
                if (param.Length < 1
                    || (socketRole =
                        channel?.Guild.Roles.FirstOrDefault(x => x.Name.ToLower() == param[0].ToLower())) == null
                    || (maxWorkNum = Convert.ToInt32(param[1])) < 1) 
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

            var memberList = socketRole.Members.ToList();
            var workList = Enumerable.Range(1, maxWorkNum).OrderBy( x=> Guid.NewGuid()).ToList();
            var workForEachMember = new List<List<int>>();

            foreach (var x in memberList)
            {
                //Initialize List for List of Array
                workForEachMember.Add(new List<int>()); 
            }

            for (int i = 0; i < workList.Count; i++)
            {
                workForEachMember[ i % memberList.Count].Add(workList[i]);
            }

            string ret = "";
            foreach (var member in memberList.OrderBy(x=> Guid.NewGuid()))
            {
                ret += $"{member.Username}: \n";
                ret += $" {string.Join(", ",workForEachMember[0].OrderBy(a=> a))} \n\n";
                workForEachMember.RemoveAt(0);
                
            }
            await Context.Channel.SendMessageAsync(null, false,
                new EmbedBuilder() {Color = Color.Blue, Description = ret}.Build());
        }
    }
}