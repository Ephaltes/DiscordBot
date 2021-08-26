using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscordBot.Core.Entity
{
    public class TimeEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public TimeSpan Time { get; set; }

        public virtual ICollection<EventEntity> EventEntities { get; set; } = new List<EventEntity>();
    }
}