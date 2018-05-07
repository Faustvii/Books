using Serilog.Core;
using Serilog.Events;
using System.Diagnostics;

namespace Books.API.Enricher
{
    public class DemystifiedStackTraceEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.Exception?.Demystify();
        }
    }
}
