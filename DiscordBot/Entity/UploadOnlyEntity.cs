using System.ComponentModel.DataAnnotations;

namespace DiscordBot.Entity
{
    public class UploadOnlyEntity
    {
        [Key]
        public ulong ChannelId { get; set; } //channel id unique globally
        
        public ulong ChannelToPostId { get; set; }
    }
}