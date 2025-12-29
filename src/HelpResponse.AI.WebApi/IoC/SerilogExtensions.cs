using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace HelpResponse.AI.WebApi.IoC
{
    public static class SerilogExtensions
    {
        public static ILoggingBuilder AddSerilog(this ILoggingBuilder logging, IConfiguration config)
        {
            LoggerSinkConfiguration writeTo = new LoggerConfiguration().ReadFrom.Configuration(config).Enrich.FromLogContext().WriteTo;
            ConsoleTheme literate = AnsiConsoleTheme.Literate;
            Log.Logger = writeTo.Console(LogEventLevel.Verbose, "[{Timestamp:HH:mm:ss} {Level:u3} {CorrelationId}] [{Message:lj} {Exception}]{NewLine}", null, null, null, literate).CreateLogger();
            logging.ClearProviders();
            logging.AddSerilog(Log.Logger);
            return logging;
        }
    }
}