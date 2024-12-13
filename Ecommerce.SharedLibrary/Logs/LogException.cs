
using Serilog;

namespace Ecommerce.SharedLibrary.Logs
{
    public static class LogException
    {
        public static void LogExceptions(Exception exception)
        {
            LogToFile(exception.Message);
            LogToConsole(exception.Message);
            LogToDebug(exception.Message);
        }

        private static void LogToFile(string message) => Log.Information(message);
        private static void LogToConsole(string message) => Log.Warning(message);
        private static void LogToDebug(string message) => Log.Debug(message);
    }
}
