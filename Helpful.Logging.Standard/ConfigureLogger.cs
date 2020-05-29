using System;
using System.Runtime.CompilerServices;
using Serilog;
using Serilog.Formatting.Compact;

namespace Helpful.Logging.Standard
{
    public class ConfigureLogger
    {
        public static void StandardSetup(string applicationName, Action appInit = null, string logFileName = "log.txt")
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File(new CompactJsonFormatter(), logFileName, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
                .Enrich.FromLogContext()
                .CreateLogger();

            LoggingContext.Set(LoggerExtensions.LOG_KEY_SOURCE, applicationName);
            Log.Logger.LogDebugWithContext("Application {ApplicationName} is starting.", applicationName);

            if (appInit != null)
            {
                try
                {
                    appInit();
                }
                catch (Exception e)
                {
                    Log.Logger.LogFatalWithContext(e, "The application {ApplicationName} failed to start due to an exception.", applicationName);
                    throw;
                }
            }
        }
    }
}
