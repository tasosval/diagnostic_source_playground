using Microsoft.Extensions.Logging;

namespace DiagnosticSourceTest.Lib
{
    public static class LoggerHelpers
    {
        /// <summary>
        /// A function that returns a <see cref="LogLevel"/> for various event names. This is supposed to be
        /// used as a callback from <see cref="DiagnosticSourceLogger"/> when we use the <see cref="ILogger{TCategoryName}"/>
        /// functionality, so we can map the various events to levels.
        ///
        /// The default behavior is that we do not log anything if we don't know the event name.
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public static LogLevel LogLevelFromEventName(string eventName)
        {
            switch (eventName)
            {
                case "FetchSuccessful":
                    return LogLevel.Information;
                case "FetchFailure":
                    return LogLevel.Error;
                default:
                    // We shouldn't be writing to the log if we don't know the message
                    return LogLevel.None;
            }
        }
    }
}