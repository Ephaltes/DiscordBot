using DSharpPlus.Entities;

namespace DiscordBot.Core.Dtos
{
    public record AddUploadOnlyChannelDto(DiscordChannel UploadChannel, DiscordChannel PostToChannel);
}