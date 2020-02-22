using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoService.State
{
    internal static class Log
    {
        internal static void LogError(string message)
        {
            try
            {
                // I'm co-opting the Visual Studio event source because I can't register
                // my own from a .VSIX installer.
                EventLog.WriteEntry("Microsoft Visual Studio",
                    $"AutoService: {(message ?? "null")}",
                    EventLogEntryType.Error);
            }
            catch
            {
                // Don't kill extension for logging errors
            }
        }
    }
}
