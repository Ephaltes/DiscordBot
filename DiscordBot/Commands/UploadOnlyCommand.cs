using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using DiscordBot.Database;
using DiscordBot.Entity;
using Serilog;

namespace DiscordBot.Modules
{
    public class UploadOnlyCommand : ModuleBase
    {
        private readonly ILogger _logger = Log.ForContext<UploadOnlyCommand>();
        private IUploadOnlyRepository _repository;

        public UploadOnlyCommand(IUploadOnlyRepository repository)
        {
            _repository = repository;
        }


        [RequireUserPermission(GuildPermission.Administrator)]
        [Command("UploadOnly")]
        public async Task UploadOnly(params string[] postToChannel)
        {
            var channelid = Context.Channel.Id;
            var serverid = Context.Guild.Id;
            ulong postToChannelId;

            try
            {
                postToChannelId = Convert.ToUInt64(postToChannel[0]);

                if (await Context.Guild.GetTextChannelAsync(postToChannelId) == null)
                {
                    await ReplyAsync("Channel not found");
                    return;
                }
            }
            catch (Exception e)
            {
                _logger.Error(e,e.Message);
                await ReplyAsync("wrong Parameter");
                return;
            }

            if (await _repository.GetByChannelId(channelid) == null)
            {
                var entity = new UploadOnlyEntity()
                    {ChannelId = channelid, ChannelToPostId = postToChannelId, ServerId = serverid};

                var result = await _repository.Add(entity);

                if (result)
                {
                    await ReplyAsync("Channel added");
                    return;
                }

                await ReplyAsync("something went wrong");
                _logger.Error("Error adding upload only");
            }
            else
            {
                await ReplyAsync("Channel already in List");
            }
        }
    }
}