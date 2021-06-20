using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using DiscordBot.Database;
using DiscordBot.Entity;
using PowerArgs;

namespace DiscordBot.Modules
{
    public class DeleteUploadOnlyCommand : ModuleBase
    {
        private readonly IUploadOnlyRepository _repository;

        public DeleteUploadOnlyCommand(IUploadOnlyRepository repository)
        {
            _repository = repository;
        }
        
        [RequireUserPermission(GuildPermission.Administrator)]
        [Command("delupload")]
        public async Task DeleteEvent(params string[] param)
        {
            try
            {
                if (param.Length < 1)
                {
                    await ReplyAsync("wrong Parameter");
                    return;
                }
                
                var uploadonlyid = new Guid(param[0]);

                if (await _repository.Delete(uploadonlyid))
                {
                    await ReplyAsync($"Uploadonly Channel deleted successful");
                    return;
                }

                await ReplyAsync("Error occurred");
            }
            catch (ArgException e)
            {
                await ReplyAsync(e.Message);
            }
        }
    }
}