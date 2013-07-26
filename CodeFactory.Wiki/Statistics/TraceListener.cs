using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace CodeFactory.Wiki.Statistics
{
    [ConfigurationElementType(typeof(CustomTraceListenerData))]
    public class TraceListener : CustomTraceListener
    {
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            LogEntry entry = data as LogEntry;

            if (entry == null)
                return;

            TraceStatistics.RegistraEvento(entry.TimeStamp.ToLocalTime(), 
                entry.ExtendedProperties["title"].ToString(),
                entry.ExtendedProperties["urlRequested"].ToString(),
                entry.ExtendedProperties["username"].ToString(), 
                (Guid)entry.ExtendedProperties["id"],
                entry.ExtendedProperties["type"].ToString());
        }

        public override void Write(string message)
        {
        }

        public override void WriteLine(string message)
        {
        }
	}
}
