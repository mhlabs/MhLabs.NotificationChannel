using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using MhLabs.DataAccessIoC.AWS;
using Newtonsoft.Json;

namespace MhLabs.NotificationChannel
{
    public class NotificationRepository : AWSRepositoryBase<IAmazonSimpleNotificationService>, INotificationRepository
    {
        protected override string AWSResourceKey { get; } = "NotificationTopic";
        public NotificationRepository(IAmazonSimpleNotificationService dataAccessClient) : base(dataAccessClient)
        {
        }

        public async Task Notify(NotificationItem item, string eventType = null)
        {
            await DataAccessClient.PublishAsync(new PublishRequest
            {
                Message = JsonConvert.SerializeObject(item),
                TopicArn = ResourceName,
                MessageAttributes = new Dictionary<string, MessageAttributeValue> { { "eventType", new MessageAttributeValue { DataType = "String", StringValue = eventType } } }
            });
        }
    }
}
