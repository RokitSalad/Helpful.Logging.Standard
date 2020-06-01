using System;

namespace Helpful.Logging.Standard
{
    public class HelpfulLoggingConfigurationException : Exception
    {
        public HelpfulLoggingConfigurationException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}