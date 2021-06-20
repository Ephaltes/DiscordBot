using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using DiscordBot.Database;
using Serilog;

namespace DiscordBot.Modules
{
    public class ClearCommand : ModuleBase
    {
        private readonly ILogger _logger = Log.ForContext<CommandHandlingService>();
        
        [RequireUserPermission(GuildPermission.Administrator)]
        [Command("clear")]
        [Alias("remove","delete")]
        public async Task Clear(params string[] amount)
        {
            int amountToDelete = 1; //Delete Message that invoked this function + amount
          
            try
            {
                amountToDelete += Convert.ToInt32(amount[0]);
                if (amountToDelete < 1)
                {
                    await ReplyAsync("wrong Parameter");
                    return;
                }
            }
            catch (Exception e)
            {
                _logger.Error(e,e.Message);
                await ReplyAsync("wrong Parameter");
                return;
            }

            Task.Run(() => DeleteTask(amountToDelete,Context.Channel.Id)); //start in other thread and let it run in background till finish
        }
        
        private async Task DeleteTask(int amount,ulong channelId)
        {
            try
            {
                const double limit = 100.0;
            
                double rounds = Math.Ceiling(amount / limit);

                for (int i = 0; i < rounds; i++)
                {
                    int tempAmount;
                
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
                        await message.DeleteAsync();
                        Task.Delay(1000).Wait();
                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error(e,e.Message);
            }
          
        }
    }
}