# DiscordBot

## Bot Privileges
- Presence Intent
- Server Members Intent

## appsettings.json Format
```json
{
  "Serilog": {
    "Using": [
      "Serilog.Sinks.Console",
      "Serilog.Sinks.File"
    ],
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "Microsoft.EntityFrameworkCore.Database.Command": "Warning"
      }
    },
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
