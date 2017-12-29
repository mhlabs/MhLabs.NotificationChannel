using System;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using MhLabs.DataAccessIoC.Abstraction;
using Newtonsoft.Json;

namespace MhLabs.NotificationChannel
{
    public class NotificationDispatcher : HandlerBase<INotificationRepository>, INotificationDispatcher
    {
        public NotificationDispatcher(INotificationRepository repository) : base(repository)
        {
        }

        public async Task Dispatch(NotificationItem item, string eventType = null)
        {
            await Repository.Notify(item, eventType);
        }
    }
}
