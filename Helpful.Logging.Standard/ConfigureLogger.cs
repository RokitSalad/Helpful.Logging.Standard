﻿using System;
using System.Reflection;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace Helpful.Logging.Standard
{
    public class ConfigureLogger
    {
        public static void StandardSetup(Action appInit = null, LogEventLevel logLevel = LogEventLevel.Information, string logFileName = "log.txt")
        {
            Assembly entryAssembly;
            string executingAssembly;
            try
            {
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Is(logLevel)
                    .WriteTo.Console()
                    .WriteTo.File(new CompactJsonFormatter(), logFileName, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
                    .Enrich.FromLogContext()
                    .CreateLogger();


                entryAssembly = Assembly.GetEntryAssembly();
                executingAssembly = entryAssembly?.FullName;
                entryAssembly.GetLogger().LogInformationWithContext("Standard setup configured for application: {ApplicationName}.", executingAssembly);

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
