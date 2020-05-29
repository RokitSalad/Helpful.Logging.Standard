using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Serilog;
using Xunit;

namespace Helpful.Logging.Standard.Tests
{
    public class TestThreadEncapsulation
    {
        /// <summary>
        /// Verifies that data injected into the LoggingContext does not leak unnecessarily between threads.
        /// Also verifies that data set on a parent thread is passed to child threads.
        /// This test works by writing a log file to disk and verifying the contents.
        /// </summary>
        [Fact]
        public void ViaTextLogFile()
        {
            ConfigureLogger.StandardSetup();
            var key = "CORRELATION_ID";
            var staticValueKey = "STATIC";
            var staticValue = "this value should be in all logs";
            LoggingContext.Set(staticValueKey, staticValue);
            Parallel.For(1, 20000, (x) =>
            {
                LoggingContext.Set(key, x);
                this.GetLogger().LogWarningWithContext("current number processing: {Number}", x);
            });
            Log.CloseAndFlush();

            var logFile = Directory.EnumerateFiles("./", "log*.txt").First();

            string source = this.GetType().ToString();
            var lines = File.ReadLines(logFile);
            foreach (string line in lines.Skip(1)) //skip the first line because it is expected to be different
            {
                var obj = JObject.Parse(line);
                Assert.StrictEqual(obj["Number"], obj[LoggerExtensions.LOG_KEY_CONTEXT][key]);
                Assert.StrictEqual(source, obj[LoggerExtensions.LOG_KEY_CONTEXT][LoggerExtensions.LOG_KEY_SOURCE]);
                Assert.StrictEqual(staticValue, obj[LoggerExtensions.LOG_KEY_CONTEXT][staticValueKey]);
            }

            File.Delete(logFile);
        }
    }
}
