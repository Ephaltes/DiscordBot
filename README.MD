# DiscordBot

## Commands
!help - displays all available commands

!uploadonly <channelID to post chats> - marks the channel for upload only, all text messages will be redirected to <channelID to post chats>
  
!clear - deletes x messages in channel, due to ratelimitations from discord deleting more than 6 messages could take a long time.

!team <groupname> <number of teams> - Creates <number of Teams> and distributing all members of <groupname> equally into the teams

## appsettings.json Format
```json
{
  "Serilog": {
    "Using": [
      "Serilog.Sinks.Console",
      "Serilog.Sinks.File"
    ],
    "MinimumLevel": "Information",
    "WriteTo": [
      {
        "Name": "Console",
        "Args": {
          "outputTemplate": "{NewLine}{Timestamp:HH:mm:ss} [{Level}] ({SourceContext}) {Message}{NewLine}{Exception}"
        }
      },
      {
        "Name": "File",
        "Args": {
          "path": "Logs/logs.txt",
          "rollingInterval": "Day",
          "outputTemplate": "{NewLine}{Timestamp:HH:mm:ss} [{Level}] ({SourceContext}) {Message}{NewLine}{Exception}"
        }
      }
    ],
    "Enrich": [
      "FromLogContext"
    ],
    "Properties": {
      "Application": "DiscordBot"
    }
  },
  "ApiToken": "myApiKey",
  "SqlitePath": "myDbName"
}

```
