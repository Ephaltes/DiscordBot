using System;
using System.Collections.Generic;

namespace DiscordBot.Entity
{
    public class TimeEntity
    {
        public TimeEntity()
        {
            EventEntities = new HashSet<EventEntity>();
        }
        public int Id { get; set; }
        public TimeSpan Time { get; set; }

        public ICollection<EventEntity> EventEntities { get; set; }
    }
}