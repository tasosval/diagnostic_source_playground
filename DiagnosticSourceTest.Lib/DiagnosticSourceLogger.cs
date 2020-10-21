using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace DiagnosticSourceTest.Lib
{
    public class DiagnosticSourceLogger: IDiagnosticSourceLogger
    {
        private readonly ILogger<DiagnosticSourceLogger> _logger;
        private readonly Func<string, LogLevel> _eventToLogLevel;
        private static readonly DiagnosticSource LoggerDiagnosticSource = new DiagnosticListener("DiagnosticSourceTest.Lib");
        public static readonly string FetchSuccessful = "FetchSuccessful";
        public static readonly string FetchFailure = "FetchFailure";
        private readonly bool _enabledILogger;

        /// <summary>
        /// If this constructor is used we only write our logs to our DiagnosticListener
        /// </summary>
        public DiagnosticSourceLogger()
        {
            _enabledILogger = false;
        }

        /// <summary>
        /// If this constructor is used (probably through a DI container) then we write our logs
        /// to the injected ILogger
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="eventToLogLevel">A callback to map event names to logger <see cref="LogLevel"/>
        ///
        /// Has a default value that returns info for successful events, error for failed events and
        /// none for everything else. See <see cref="LoggerHelpers.LogLevelFromEventName"/> for the default
        /// implementation</param>
        public DiagnosticSourceLogger(ILogger<DiagnosticSourceLogger> logger, 
            Func<string, LogLevel> eventToLogLevel = null)
        {
            _logger = logger;
            _eventToLogLevel = eventToLogLevel ?? LoggerHelpers.LogLevelFromEventName;
            _enabledILogger = true;
        }
        
        /// <summary>
        /// The only way to interact with the <see cref="DiagnosticSourceLogger"/>
        /// We use this to abstract logging. 
        /// </summary>
        /// <param name="eventName">A characteristic event name that can be used to specify the importance of the
        /// logged item and to give the ability to consumers to use it or not</param>
        /// <param name="o">An object with the data we want to log</param>
        public void Log(string eventName, object o)
        {
            if (_enabledILogger)
            {
                LogWithILogger(eventName, o);
            }
            else
            {
                LogWithDiagnosticSource(eventName, o);    
            }
        }

        private void LogWithDiagnosticSource(string eventName, object o)
        {
            if (LoggerDiagnosticSource.IsEnabled(eventName))
            {
                LoggerDiagnosticSource.Write(eventName, o);
            }
        }

        private void LogWithILogger(string eventName, object o)
        {
            _logger?.Log(_eventToLogLevel(eventName), o.ToString());
        }
    }

    public interface IDiagnosticSourceLogger
    {
        void Log(string eventName, object o);
    }
}