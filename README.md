# DiagnosticSource Playground

A small collection of C# projects to experiment with and learn about using DiagnosticSource and Logger 
abstractions to instrument libraries, so that their information can be consumed by client code
to create logs, etc.

The projects inside this solution are
1. DiagnosticSourceTest.Lib - A .netstandard 2.0 library that contains a hypothetical service that leaves some traces 
during execution. 
2. DiagnosticSource.Console48 - A .NET 4.8 console project that uses the aforementioned library and 
logs with log4net the events that are created
3. DiagnosticSource.ConsoleCore - A dotnetcore 3.1 console project that uses the aforementioned library 
and logs with serilog the events that are created, or injects an ILogger to the library and let's it
log on it's own
4. DiagnosticSourceTest.Lib.Hosting - A library that contains extensions to configure the 
*DiagnosticSourceTest.Lib* in a dotnetcore host-type application 
 