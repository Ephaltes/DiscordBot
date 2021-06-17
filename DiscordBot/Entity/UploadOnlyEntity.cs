namespace DiscordBot.Entity
{
    public class UploadOnlyEntity
    {
        public ulong ChannelId { get; set; } //channel id unique globally
        
        public ulong ChannelToPostId { get; set; }
    }
}