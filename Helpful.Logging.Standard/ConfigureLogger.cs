using Serilog;
using Serilog.Formatting.Compact;

namespace Helpful.Logging.Standard
{
    public class ConfigureLogger
    {
        public static void StandardSetup(string logFileName = "log.txt")
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File(new CompactJsonFormatter(), logFileName, rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
                .Enrich.FromLogContext()
                .CreateLogger();
        }
    }
}
