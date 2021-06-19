using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscordBot.Entity
{
    public class EventEntity
    {
        public EventEntity()
        {
            TimeEntities = new HashSet<TimeEntity>();
        }
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public ulong ChannelToPostId { get; set; }
        
        public ulong ServerId { get; set; }
        public virtual ICollection<TimeEntity> TimeEntities { get; set; }
    }
}