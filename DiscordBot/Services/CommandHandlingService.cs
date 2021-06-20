using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Database;
using DiscordBot.Entity;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordBot
{
    public class CommandHandlingService
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;
        private readonly IUploadOnlyRepository _uploadOnlyRepository;
        private readonly IEventRepository _eventRepository;
        private Timer _eventTimer;

        public CommandHandlingService(IServiceProvider services, DiscordSocketClient discord, CommandService commands, IUploadOnlyRepository uploadOnlyRepository, IEventRepository eventRepository)
        {
            _services = services;
            _discord = discord;
            _commands = commands;
            _uploadOnlyRepository = uploadOnlyRepository;
            _eventRepository = eventRepository;

            // Hook CommandExecuted to handle post-command-execution logic.
            _commands.CommandExecuted += CommandExecutedAsync;
            
            // Hook MessageReceived so we can process each message to see
            // if it qualifies as a command.
            _discord.MessageReceived += MessageReceivedAsync;

            InitTimer();
        }

        private void InitTimer()
        {
            _eventTimer = new Timer();
            _eventTimer.AutoReset = false;
            _eventTimer.Enabled = true;
            _eventTimer.Interval = 1000 * 15;// 15 sekunden
            _eventTimer.Elapsed += CheckForEvents;
        }

        private async void CheckForEvents(Object source, ElapsedEventArgs e)
        {
            _eventTimer.Stop();
            try
            {
                var eventList = await _eventRepository.GetAll();
                if (eventList.Count < 1)
                {
                    _eventTimer.Start();
                    return;
                }

                foreach (var eventEntity in eventList)
                {
                    if (await MessageForEventSent(eventEntity, DateTime.Now,
                        $"{eventEntity.Name} is now!"))
                    {
                        await _eventRepository.Delete(eventEntity.Id);
                        _eventTimer.Start();
                        return;
                    }

                    foreach (var reminderTime in eventEntity.TimeEntities.ToList()) //ToList Because we are modifiyng eventEntity
                    {
                        if (!await MessageForEventSent(eventEntity,
                            DateTime.Now.Add(reminderTime.Time),
                            $"Event '{eventEntity.Name}' is in about {reminderTime.Time} !\n" +
                            $"on {eventEntity.Date.ToShortDateString()} " +
                            $"{eventEntity.Date.ToShortTimeString()}")) continue;

                        eventEntity.TimeEntities.Remove(reminderTime);
                        await _eventRepository.Update(eventEntity);
                    }
                }
            }
            catch (Exception exception)
            {
                await LoggingService.Log(new LogMessage(LogSeverity.Critical, nameof(CommandHandlingService), "EventTimer",
                    exception));
            }
            _eventTimer.Start();
        }

        private async Task<bool> MessageForEventSent(EventEntity entity,DateTime timeToSend, string message)
        {
            if (entity.Date < timeToSend)
            {
                var channel = _discord.GetGuild(entity.ServerId).GetTextChannel(entity.ChannelToPostId);
                await channel.SendMessageAsync("", false,
                    new EmbedBuilder() {Description = message, Color = Color.Purple}.Build());

                return true;
            }
            return false;
        }
        
        public async Task InitializeAsync()
        {
            // Register modules that are public and inherit ModuleBase<T>.
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }
        
        private async Task<bool> IsMessageInUploadOnlyChannel(SocketMessage msg, IUploadOnlyRepository repository)
        {
            var channel = msg.Channel as SocketGuildChannel;

            var uploadOnlyEntity = await repository.GetByChannelId(channel.Id);
            
            
            if (msg.Author.IsBot 
                || uploadOnlyEntity == null 
                ||  msg.Attachments.Count > 0)
                return false;


            var channelToPost = channel.Guild.TextChannels.FirstOrDefault(x => x.Id == uploadOnlyEntity.ChannelToPostId);

            if (channelToPost == null)
            {
                await msg.Channel.SendMessageAsync("Channel To Post to doesn´t exist.");
                return true;
            }

            EmbedBuilder builder = new EmbedBuilder();

            builder.WithAuthor(msg.Author);
            builder.WithTimestamp(msg.Timestamp);
            builder.WithDescription(msg.Content);
            builder.Color = Color.Gold;

            await channelToPost.SendMessageAsync("", false, builder.Build());
            await msg.DeleteAsync();

            return true;
        }

        public async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            if (await IsMessageInUploadOnlyChannel(rawMessage, _uploadOnlyRepository))
                return;
            
            // Ignore system messages, or messages from other bots
            if (!(rawMessage is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;

            // This value holds the offset where the prefix ends
            var argPos = 0;
            // Perform prefix check. You may want to replace this with
            // (!message.HasCharPrefix('!', ref argPos))
            // for a more traditional command format like !help.
            //if (!message.HasMentionPrefix(_discord.CurrentUser, ref argPos)) return;
            if (!message.HasCharPrefix('!', ref argPos)) return;

            var context = new SocketCommandContext(_discord, message);
            // Perform the execution of the command. In this method,
            // the command service will perform precondition and parsing check
            // then execute the command if one is matched.
            await _commands.ExecuteAsync(context, argPos, _services);
            // Note that normally a result will be returned by this format, but here
            // we will handle the result in CommandExecutedAsync,
        }

        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // command is unspecified when there was a search failure (command not found); we don't care about these errors
            if (!command.IsSpecified)
                return;

            // the command was successful, we don't care about this result, unless we want to log that a command succeeded.
            if (result.IsSuccess)
                return;

            // the command failed, let's notify the user that something happened.
            await context.Channel.SendMessageAsync($"error: {result}");
        }
    }
}
