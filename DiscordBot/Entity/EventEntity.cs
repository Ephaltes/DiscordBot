using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DiscordBot.Entity
{
    public class EventEntity
    {
        public EventEntity()
        {
            TimeEntities = new HashSet<TimeEntity>();
        }
        
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public ulong ChannelToPostId { get; set; }
        public virtual ICollection<TimeEntity> TimeEntities { get; set; }
    }
}