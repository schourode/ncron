using System.Diagnostics;
using System.Reflection;
using NCron.Service;

namespace NCron
{
    internal static class ExceptionHelper
    {
        // We explicitly use the EventLog for logging purposes in the main exception handling.
        // If the configuration cannot be loaded - and no log factory created - it will be logged.
        // If a custom ILog implementation throws, this will also be logged here.
        private static readonly EventLog CoreLog = new EventLog { Source = ApplicationInfo.ApplicationName };

        internal static void LogUnhandledException(object exception) {
            CoreLog.WriteEntry("An unhandled exception has occured. " + exception, EventLogEntryType.Error);
        }
    }
}