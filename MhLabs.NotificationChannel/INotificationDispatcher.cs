using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MhLabs.NotificationChannel
{
    public interface INotificationDispatcher
    {
        Task Dispatch(NotificationItem item, string eventType = null);
    }
}
