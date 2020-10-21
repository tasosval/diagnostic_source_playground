using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace DiagnosticSource.ConsoleCore
{
    public class DiagnosticListenerToLogger: IDisposable, IObserver<KeyValuePair<string, object>>, IObserver<DiagnosticListener>
    {
        private const string DiagnosticSourceTestLibListenerName = "DiagnosticSourceTest.Lib";
        private readonly ILogger<DiagnosticListenerToLogger> _logger;
        private static readonly object AllListeners = new object(); 
        
        private IDisposable _diagnosticSourceLibSubscription = null;

        private readonly IDisposable _listenerSubscription;

        public DiagnosticListenerToLogger(ILogger<DiagnosticListenerToLogger> logger)
        {
            _logger = logger;
            _listenerSubscription  = DiagnosticListener.AllListeners.Subscribe(this);
        }
        
        public void Dispose()
        {
            _diagnosticSourceLibSubscription?.Dispose();
            _listenerSubscription?.Dispose();
        }
        
        public void OnCompleted()
        {
            
        }

        public void OnError(Exception error)
        {
            _logger.LogError(error, "error with the provider");
        }

        public void OnNext(DiagnosticListener listener)
        {
            _logger.LogDebug($"Got a listener with name {listener.Name}");
            if (listener.Name != DiagnosticSourceTestLibListenerName) return;

            lock (AllListeners)
            {
                _diagnosticSourceLibSubscription?.Dispose();

                _diagnosticSourceLibSubscription = listener.Subscribe(this);
            }
        }

        public void OnNext(KeyValuePair<string, object> evnt)
        {
            var (key, value) = evnt;
            switch (key)
            {
                case "FetchFailure":
                    _logger.LogError(
                        $"From Listener {DiagnosticSourceTestLibListenerName} Received Event {key} with payload {value.ToString()}");
                    break;
                case "FetchSuccessful":
                    _logger.LogInformation(
                        $"From Listener {DiagnosticSourceTestLibListenerName} Received Event {key} with payload {value.ToString()}");
                    break;
            }
        }
    }
}