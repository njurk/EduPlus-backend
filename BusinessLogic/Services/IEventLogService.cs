namespace BusinessLogic.Services
{
    public interface IEventLogService
    {
        void Log(string type, int? userId = null, string? email = null, IEnumerable<int>? roleLevels = null, string? reason = null, Exception? ex = null);
    }

    public class EventLogService : IEventLogService
    {
        private readonly string _logsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
        private readonly object _lock = new();

        public EventLogService() => Directory.CreateDirectory(_logsDir);

        public void Log(string type, int? userId = null, string? email = null, IEnumerable<int>? roleLevels = null, string? reason = null, Exception? ex = null)
        {
            var roles = roleLevels != null ? string.Join(",", roleLevels.OrderBy(r => r)) : "?";
            var user = userId.HasValue ? $"r:{roles} {email ?? $"id:{userId}"}" : (email ?? "-");
            var entry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} || {type,-10} | {user,-35}{(reason != null ? $" | {reason}" : "")}{(ex != null ? $" [{ex.GetType().Name}]" : "")}";

            lock (_lock)
            {
                File.AppendAllText(Path.Combine(_logsDir, $"events_{DateTime.Now:yyyy-MM-dd}.log"), entry + Environment.NewLine);
                if (ex?.StackTrace != null)
                    File.AppendAllText(Path.Combine(_logsDir, $"events_{DateTime.Now:yyyy-MM-dd}.log"), $"    {ex.StackTrace}{Environment.NewLine}");
            }
        }
    }
}
