using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Helpful.Logging.Standard
{
    public static class LoggingContext
    {
        private static readonly ConcurrentDictionary<string, AsyncLocal<object>> AsyncLocalContext = new ConcurrentDictionary<string, AsyncLocal<object>>();

        public static IDictionary<string, object> LoggingScope =>
            AsyncLocalContext.ToDictionary(o => o.Key, o => o.Value?.Value);

        public static void Set(string key, object value) =>
            AsyncLocalContext.GetOrAdd(key, _ => new AsyncLocal<object>()).Value = value;

        public static object Get(string key) =>
            AsyncLocalContext.TryGetValue(key, out AsyncLocal<object> result) ? result.Value : null;

        public static object Delete(string key) =>
            AsyncLocalContext.TryRemove(key, out AsyncLocal<object> deleted) ? deleted.Value : null;
    }
}
