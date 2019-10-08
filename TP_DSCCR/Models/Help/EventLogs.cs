using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TP_DSYNC.Models.Help
{
    public static class EventLogs
    {
        public static void Write(string Message,int EventId, EventLogEntryType EventLogEntryType)
        {
            try
            {
                if (!EventLog.SourceExists(Program.EventLogSource))
                {
                    EventLog.CreateEventSource(Program.EventLogSource, "SensorData");
                }
                EventLog.WriteEntry(Program.EventLogSource, Message, EventLogEntryType, EventId);
            }
            catch
            {
            }
        }
    }
}
