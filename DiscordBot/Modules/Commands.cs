using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace DiscordBot.Modules
{
    public class Commands : ModuleBase
    {
        [Command("help")]
        public async Task TestenCommand(params string[] param)
        {
            string ret;

            string version = "Version 2.0.0\n";
            string uploadonly = "!uploadonly <ChannelID To Post to> : This Text send into this Channel will be Transfered to ChannelID Channel\n\n";
            string clear = "!clear <amount to delete> : Deletes x messages in channel, " +
                                "deleting more than 6 Messages will get into RateLimit --> taking very long time due API rate limit\n\n";
            string teamscrambler = "!teamscrambler <rolename To scramble> <amount of Teams> : Scrambles Member of Role into amount of Teams\n\n";


            ret = version + uploadonly+clear+teamscrambler;
            
            await ReplyAsync("",false,new EmbedBuilder(){Description = ret,Color = Color.Purple}.Build());
        }

        [RequireUserPermission(GuildPermission.Administrator)]
        [Command("UploadOnly")]
        public async Task UploadOnly(params string[] postToChannel)
        {
            // get user info from the Context
            var user = Context.User;
            var channelid = Context.Channel.Id;
            ulong postToChannelId;

            try
            {
                postToChannelId = Convert.ToUInt64(postToChannel[0]);

                if (await Context.Guild.GetTextChannelAsync(postToChannelId) == null)
                {
                    throw new Exception();
                }
            }
            catch (Exception e)
            {
                await ReplyAsync("wrong Parameter");
                return;
            }
            
            SavedSettings settings = SavedSettings.settings;
            if (!settings.GetUploadOnlyList().ContainsKey(channelid))
            {
                settings.AddToUploadOnlyList(channelid, postToChannelId);
                await ReplyAsync("Channel added");
            }
            else
            {
                await ReplyAsync("Channel already in List");
            }
        }
        
        
        [RequireUserPermission(GuildPermission.Administrator)]
        [Command("clear")]
        public async Task delHistory(params string[] amount)
        {
            var channel = Context.Channel as SocketGuildChannel;
            int amountToDelete = 1; //Delete Message that invoked this function + amount
          
            try
            {
                amountToDelete += Convert.ToInt32(amount[0]);
                if(amountToDelete < 1)
                    throw new Exception();
            }
            catch (Exception e)
            {
                await ReplyAsync("wrong Parameter");
                return;
            }

            Task.Run(() => deleteTask(amountToDelete));
        }

        private async Task deleteTask(int amount)
        {
            const int LIMIT = 100;
            
            int rounds = amount / LIMIT;

            if (amount % LIMIT != 0)
                rounds++;

            for (int i = 0; i < rounds; i++)
            {
                int tempAmount = 0;
                if (amount > 100)
                {
                    amount -= 100;
                    tempAmount = 100;
                }
                else
                {
                    tempAmount = amount;
                }
                
                var messages = await  Context.Channel.GetMessagesAsync(tempAmount).FlattenAsync();

                foreach (var message in messages.ToArray())
                {
                    await Context.Channel.DeleteMessageAsync(message);
                    Task.Delay(500).Wait();
                }
            }
        }
        
        [RequireUserPermission(GuildPermission.Administrator)]
        [Command("TeamScrambler")]
        public async Task TeamScrambler(params string[] param)
        {
            var channel = Context.Channel as SocketGuildChannel;
            SocketRole socketRole;
            int teamCount = 0;
            try
            {
                if (param.Length < 2 
                    || (socketRole = channel.Guild.Roles.FirstOrDefault(x => x.Name.ToLower() == param[0].ToLower())) == null)
                {
                    throw new Exception("Wrong arguments");
                }

                teamCount = Convert.ToInt32(param[1]);
                if(teamCount < 1)
                    throw new Exception();
            }
            catch (Exception e)
            {
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