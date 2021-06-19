using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscordBot.Entity
{
    public class UploadOnlyEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        public ulong ServerId { get; set; }
        public ulong ChannelId { get; set; } //channel id unique globally
        
        public ulong ChannelToPostId { get; set; }
    }
}