using System;
using System.Reflection;
using Serilog;

namespace Helpful.Logging.Standard
{
    public static class LoggerExtensions
    {
        public const string LOG_KEY_SOURCE = "SOURCE";
        public const string LOG_KEY_CONTEXT = "CONTEXT";

        public static ILogger GetLogger(this object loggingSource)
        {
            switch (loggingSource)
            {
                case Assembly a:
                    LoggingContext.Set(LOG_KEY_SOURCE, a.FullName);
                    break;
                case { } s:
                    LoggingContext.Set(LOG_KEY_SOURCE, s.GetType());
                    break;
            }
            return Log.Logger;
        }

        public static void LogDebugWithContext(this Serilog.ILogger logger, string message, params object[] args)
        {
            SetContext();
            Log.Debug(message, args);
        }

        public static void LogInformationWithContext(this Serilog.ILogger logger, string message, params object[] args)
        {
            SetContext();
            Log.Information(message, args);
        }

        public static void LogWarningWithContext(this Serilog.ILogger logger, string message, params object[] args)
        {
            SetContext();
            Log.Warning(message, args);
        }

        public static void LogErrorWithContext(this Serilog.ILogger logger, Exception exception, string message, params object[] args)
        {
            SetContext();
            Log.Error(exception, message, args);
        }

        public static void LogFatalWithContext(this Serilog.ILogger logger, Exception exception, string message, params object[] args)
        {
            SetContext();
            Log.Fatal(exception, message, args);
        }

        private static void SetContext()
        {
            Serilog.Context.LogContext.PushProperty(LOG_KEY_CONTEXT, LoggingContext.LoggingScope, true);
        }
    }
}
