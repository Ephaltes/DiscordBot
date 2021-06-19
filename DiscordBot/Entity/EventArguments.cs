using System;
using System.Collections.Generic;
using Microsoft.VisualBasic;
using PowerArgs;
using TimeSpanParserUtil;

namespace DiscordBot.Entity
{
    public class EventArguments
    {
        [ArgRequired]
        [ArgPosition(0)]
        [ArgDescription("Name of the Event")]
        [ArgShortcut("-n")]
        public string Name { get; set; }

        [ArgRequired]
        [ArgPosition(1)]
        [ArgDescription("Date of the Event")]
        [ArgShortcut("-d")]
        public DateTime Date { get; set; }
        
        [ArgRequired]
        [ArgPosition(2)]
        [ArgDescription("Time of the Event")]
        [ArgShortcut("-t")]
        public TimeSpan Time { get; set; }

        [ArgRequired]
        [ArgPosition(3)]
        [ArgDescription("Channel to Post Reminder To")]
        [ArgShortcut("-c")]
        public ulong ChannelToPostId { get; set; }

        [ArgPosition(4)]
        [ArgDescription("Array of Reminders to occur before Date of the Event")]
        [ArgShortcut("-r")]
        public List<string> RemindersAsString
        {
            get
            {
                List<string> list = new List<string>();

                foreach (var time in Reminders)
                {
                    list.Add(time.ToString());
                }

                return list;
            }
            set
            {
                List<TimeEntity> list = new List<TimeEntity>();

                try
                {
                    foreach (var time in value)
                    {
                        var entity = new TimeEntity();
                        entity.Time = TimeSpanParser.Parse(time);
                        list.Add(entity);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception("Could not parse ReminderTime");
                }

                Reminders = list;
            }
        }


        [ArgIgnore]
        public List<TimeEntity> Reminders { get; set; }

        public EventEntity ToEntity()
        {
            var entity = new EventEntity
            {
                Date = Date.Add(Time),
                Name = Name,
                TimeEntities = Reminders,
                ChannelToPostId = ChannelToPostId
            };
            return entity;
        }
    }
}