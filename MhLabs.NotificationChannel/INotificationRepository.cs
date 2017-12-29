using System.Threading.Tasks;

namespace MhLabs.NotificationChannel
{
    public interface INotificationRepository
    {
        Task Notify(NotificationItem item, string eventType = null);
    }
}
