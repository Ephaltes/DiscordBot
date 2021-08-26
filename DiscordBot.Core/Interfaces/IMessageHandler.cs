using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;

namespace DiscordBot.Core.Interfaces
{
    public interface IMessageHandler
    {
        public Task MessageReceived(DiscordClient client, MessageCreateEventArgs messageEvent);
        public Task DeleteIfMessageIsInUploadOnlyChannel(MessageCreateEventArgs messageEvent);
        public Task<bool> IsMessageInUploadOnlyChannel(MessageCreateEventArgs messageEvent);
        public Task RedirectMessageFromUploadOnlyToPostChannel(DiscordMessage message);
    }
}