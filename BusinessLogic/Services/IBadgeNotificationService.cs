using System.Collections.Concurrent;

namespace BusinessLogic.Services
{
    public interface IBadgeNotificationService
    {
        event Action<int>? OnBadgeChanged;
        void NotifyBadgeChanged(int userId);
    }

    public class BadgeNotificationService : IBadgeNotificationService
    {
        public event Action<int>? OnBadgeChanged;

        public void NotifyBadgeChanged(int userId)
        {
            OnBadgeChanged?.Invoke(userId);
        }
    }
}
