namespace BusinessLogic.Services
{
    public interface IEventLogService
    {
        void LogEvent(string eventType, int? userId, string? email, int? roleLevel = null, string? viewName = null, string? details = null);
        void LogError(string source, string message, Exception? exception = null);
    }

    public class EventLogService : IEventLogService
    {
        private readonly string _logsDirectory;
        private readonly object _lock = new();

        public EventLogService()
        {
            _logsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            if (!Directory.Exists(_logsDirectory))
            {
                Directory.CreateDirectory(_logsDirectory);
            }
        }

        public void LogEvent(string eventType, int? userId, string? email, int? roleLevel = null, string? viewName = null, string? details = null)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var userInfo = userId.HasValue ? $"r:{roleLevel ?? 0} {email}" : "anonymous";
            var view = viewName ?? "";
            var logEntry = $"{timestamp} || {eventType,-15} | {view,-20} | {userInfo,-35} | {details ?? ""}";

            WriteToFile("events", logEntry);
        }

        public void LogError(string source, string message, Exception? exception = null)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            var exceptionDetails = exception != null ? $" | {exception.GetType().Name}: {exception.Message}" : "";
            var logEntry = $"{timestamp} | ERROR | {source,-30} | {message}{exceptionDetails}";

            WriteToFile("errors", logEntry);

            if (exception?.StackTrace != null)
            {
                WriteToFile("errors", $"    StackTrace: {exception.StackTrace}");
            }
        }

        private void WriteToFile(string prefix, string content)
        {
            var fileName = $"{prefix}_{DateTime.Now:yyyy-MM-dd}.log";
            var filePath = Path.Combine(_logsDirectory, fileName);

            lock (_lock)
            {
                File.AppendAllText(filePath, content + Environment.NewLine);
            }
        }
    }
}
