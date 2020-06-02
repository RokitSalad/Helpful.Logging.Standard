using System;
using System.Reflection;
using Serilog;
using Serilog.Formatting.Compact;

namespace Helpful.Logging.Standard
{
    public class ConfigureLogger
    {
        public static void StandardSetup(Action appInit = null, string logFileName = "log.txt")
        {
            Assembly entryAssembly;
            string executingAssembly;
            try
            {
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .WriteTo.Console()
                    .WriteTo.File(new CompactJsonFormatter(), logFileName, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
                    .Enrich.FromLogContext()
                    .CreateLogger();


                entryAssembly = Assembly.GetEntryAssembly();
                executingAssembly = entryAssembly?.FullName;
                entryAssembly.GetLogger().LogInformationWithContext("Application {ApplicationName} is starting.", executingAssembly);

            }
            catch (Exception e)
            {
                throw new HelpfulLoggingConfigurationException("Failed to configure Helpful.Logging.Standard.", e);
            }

            if (appInit != null)
            {
                try
                {
                    appInit();
                }
                catch (Exception e)
                {
                    entryAssembly.GetLogger().LogFatalWithContext(e, "The application {ApplicationName} failed to start due to an exception.", executingAssembly);
                    throw;
                }
            }
        }
    }
}
