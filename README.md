# Helpful.Logging.Standard
DotNet Standard incarnation of Helpful.Logging
## Quick Start
Install with:
```
Install-Package Helpful.Logging.Standard
```
Initialise with:
```c#
ConfigureLogger.StandardSetup();
```
Use anywhere in your code with:
```c#
this.GetLogger().LogInformationWithContext("Some information to log with a {Parameter}", "Parameter Value");
```
Or log a caught exception with:
```c#
catch(Exception e)
{
    this.GetLogger().LogErrorWithContext(e, "Some information to log with a {Parameter}", "Parameter Value");
}
```
Add properties to be output into each log entry with:
```c#
LoggingContext.Set("CorrelationId", Guid.NewGuid());
```
Each line logged looks like this:
```json
{"@t":"2020-05-29T05:40:17.8610634Z","@mt":"Some information to log with a {Parameter}","@l":"Information","Parameter":"Parameter Value","CONTEXT":{"CorrelationId":"51aa4171-ab47-4e22-8abe-ad273b07ac9c","SOURCE":"SampleWebApp.Controllers.WeatherForecastController"}}
```
## About
Helpful.Logging.Standard implements [Serilog](https://github.com/serilog/serilog) and leverages the in built LogContext feature to add values for inclusion to every log entry. An example of where this is useful might be with correlation id's passed between services to allow cross-service log analysis.

Any data added to the Helpful.Logging.Standard.LoggingContext will appear in the "CONTEXT" object of each log entry.

The LoggingContext uses LocalAsync so previously set values will persist into new threads, but subsequent changes will not propegate back.

If you don't want to use Serilog or you don't want your logs in a json format, this library is not for you.

## Build Status
[![Build Status](https://dev.azure.com/pete0159/Helpful.Libraries/_apis/build/status/RokitSalad.Helpful.Logging.Standard?branchName=master)](https://dev.azure.com/pete0159/Helpful.Libraries/_build/latest?definitionId=5&branchName=master)