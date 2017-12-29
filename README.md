# MhLabs.NotificationChannel

## Purpose
To provide an easier and standardised pub/sub pattern between our services.

## Synopsis
Each opted-in service gets its own SNS topic, named the same as the stack. Note that we're setting the physical ID of the stack through the `TopicName` property. This so we can get looser coupled services than we'd get if we used cloudformation exports. 
I.e, stack `order-placement` will create a topic called `order-placement`, which will work as a pub/sub channel for all events happening in the `order-placement` stack.

The type of event is added as a `MessageAttribute` on the published item. Consumers can subscribe to the `order-placement` topic and filter on `MessageAttributes` so that we can have one lambda _only_ receiving `eventType: create` for newly created orders and another one receiving `eventType: update` for updated orders, etc. See http://docs.aws.amazon.com/sns/latest/dg/message-filtering.html for more on filtering.

The `MhLabs.NotificationChannel` package provides helper methods and dependency injection that helps enforce this strategy.

## Usage

### serverless.template
```
[...]
"NotificationTopic": {
	"Type":"AWS::SNS::Topic",
	"Properties": {
		"TopicName":{"Ref": "AWS::StackName"}
	}
}
[...]
```
...and in your `AWS::Serverless::Function`-resource

```
"YourLambdaFunction": {
 "Type": "AWS::Serverless::Function",
  "Properties": {
    [...]
    "Environment": {
      "Variables": {
        "NotificationTopic": { "Ref": "NotificationTopic" }
      }
      [...]
    }
  }
}
```

Add package: `Install-Package MhLabs.NotificationChannel`

In Startup.cs (if service is asp.net mvc)

Make sure the following is present in `ConfigureServices`:
```
services.AddAWSRepositories<Startup>();
services.AddHandlers<Startup>();
```

If you need to declare the notification dispatcher without DI: (treat as a single instance)
```
_notificationDispatcher = new NotificationDispatcher(new NotificationRepository(new AmazonSimpleNotificationServiceClient(Region.Current)));
```

To publish a message:
```
await _notificationDispatcher.Dispatch(new NotificationItem { EventBody = item.Id }, "create");
```
