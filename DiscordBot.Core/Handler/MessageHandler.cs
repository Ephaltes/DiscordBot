using System.Linq;
using System.Threading.Tasks;
using DiscordBot.Core.Entity;
using DiscordBot.Core.Interfaces;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using Serilog;

namespace DiscordBot.Core.Handler
{
    public class MessageHandler : IMessageHandler
    {
        private readonly ILogger _logger;
        private readonly IUploadOnlyRepository _uploadOnlyRepository;
        public MessageHandler(IUploadOnlyRepository uploadOnlyRepository, ILogger logger)
        {
            _uploadOnlyRepository = uploadOnlyRepository;
            _logger = logger.ForContext(GetType());
        }
        public async Task MessageReceived(DiscordClient client, MessageCreateEventArgs messageEvent)
        {
            _ = Task.Run(async () => await DeleteIfMessageIsInUploadOnlyChannel(messageEvent));
        }

        public async Task DeleteIfMessageIsInUploadOnlyChannel(MessageCreateEventArgs messageEvent)
        {
            bool isInUploadOnlyChannel = await IsMessageInUploadOnlyChannel(messageEvent);

            if (isInUploadOnlyChannel)
            {
                await RedirectMessageFromUploadOnlyToPostChannel(messageEvent.Message);
                await messageEvent.Message.DeleteAsync();
            }
        }

        public async Task<bool> IsMessageInUploadOnlyChannel(MessageCreateEventArgs messageEvent)
        {
            DiscordChannel? channel = messageEvent.Channel;

            UploadOnlyEntity? uploadOnlyEntity = await _uploadOnlyRepository.GetByChannelId(channel.Id);


            if (messageEvent.Author.IsBot
                || uploadOnlyEntity == null
                || messageEvent.Message.Attachments.Count > 0)
                return false;

            return true;
        }

        public async Task RedirectMessageFromUploadOnlyToPostChannel(DiscordMessage message)
        {
            DiscordChannel? channel = message.Channel;

            UploadOnlyEntity? uploadOnlyEntity = await _uploadOnlyRepository.GetByChannelId(channel.Id);

            DiscordChannel? channelToPost =
                channel.Guild.Channels.FirstOrDefault(x => x.Value.Id == uploadOnlyEntity?.ChannelToPostId).Value;

            if (channelToPost == null)
            {
                _logger.Warning("Channel to post does not exist");

                return;
            }

            DiscordEmbedBuilder builder = new DiscordEmbedBuilder();
            builder.WithAuthor(message.Author.Username);
            builder.WithTimestamp(message.Timestamp);
            builder.WithDescription(message.Content);

            await channelToPost.SendMessageAsync(builder);
        }
    }
}