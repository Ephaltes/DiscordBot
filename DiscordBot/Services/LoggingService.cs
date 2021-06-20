using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Discord;

namespace DiscordBot
{
    public class LoggingService
    {
      public static Task Log(LogMessage log)
      {
                 string msg = $"{log.Source}: {log.Message} {log.Exception}";
                 switch (log.Severity)
                 {
                     case LogSeverity.Critical:
                         Serilog.Log.Fatal(msg);
                         break;
                     case LogSeverity.Error:
                         Serilog.Log.Error(msg);
                         break;
                     case LogSeverity.Warning:
                         Serilog.Log.Warning(msg);
                         break;
                     case LogSeverity.Info:
                         Serilog.Log.Information(msg);
                         break;
                     case LogSeverity.Verbose:
                         Serilog.Log.Verbose(msg);
                         break;
                     case LogSeverity.Debug:
                         Serilog.Log.Debug(msg);
                         break;
                 }
                 return Task.CompletedTask;
             }
    }
}