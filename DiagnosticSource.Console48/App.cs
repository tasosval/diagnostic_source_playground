using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DiagnosticSourceTest.Lib;
using log4net;

namespace DiagnosticSource.Console48
{
    public class App: IDisposable
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));
        
        /// <summary>
        /// Synchronization object
        /// </summary>
        private static readonly object allListeners = new object(); 

        /// <summary>
        /// This is the subscription reference that we will use to get the messages from the "DiagnosticSourceTest.Lib"
        /// <see cref="DiagnosticSource"/>
        /// </summary>
        private static IDisposable _diagnosticSourceSubscription = null;

        /// <summary>
        /// This is the subscription to the AllListeners observable, where each <see cref="DiagnosticSource"/> which
        /// will emit events eventually shows up.
        ///
        /// We use this as an entrypoint to get hold of the <see cref="DiagnosticSource"/> we are interested in.
        ///
        /// We use the <see cref="ObservableExtensions"/> to Subscribe with a simple delegate
        /// (instead of implementing the <see cref="IObserver{DiagnosticListener}"/> interface) 
        /// </summary>
        private static readonly IDisposable _listenerSubscription = DiagnosticListener.AllListeners
            .Subscribe(delegate (DiagnosticListener listener)
        {
            log.Debug($"Got a listener with name {listener.Name}");
            if (listener.Name != "DiagnosticSourceTest.Lib") return;
            
            // In the off chance we get a call from multiple threads, make sure that the initialization of the
            // subscription to the DiagnosticSource only happens once 
            lock(allListeners)
            {
                // Get rid of any previous subscriptions
                _diagnosticSourceSubscription?.Dispose();

                // Create a new subscription and offer a delegate to call for every event that this DiagnosticSource
                // emits
                _diagnosticSourceSubscription = listener.Subscribe(evnt =>
                {
                    switch (evnt.Key)
                    {
                        case "FetchFailure":
                            log.Error(
                                $"From Listener {listener.Name} Received Event {evnt.Key} with payload {evnt.Value}");
                            break;
                        case "FetchSuccessful":
                            log.Info(
                                $"From Listener {listener.Name} Received Event {evnt.Key} with payload {evnt.Value}");
                            break;
                    }
                });
            }
        });

        public async Task Run()
        {
            // No dependency injection here. We just create the logger by hand and feed it to the service
            IDiagnosticSourceLogger diagnosticSourceLogger = new DiagnosticSourceLogger();
            ITestService service = new TestService(diagnosticSourceLogger);
            
            // Make some actions with the service (should create the appropriate events)
            await service.FetchItems("http://www.in.gr");
            await service.FetchItems("www.in.gr");
            
            log.Info("finished execution");
        }

        public void Dispose()
        {
            _diagnosticSourceSubscription?.Dispose();
            _listenerSubscription?.Dispose();
        }
    }
}