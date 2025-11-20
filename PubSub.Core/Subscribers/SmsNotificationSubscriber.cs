namespace PubSub.Core.Subscribers;

/// <summary>
/// مشترک برای ارسال پیامک هنگام انتشار خبر جدید
/// </summary>
public class SmsNotificationSubscriber : ISubscriber<Events.NewsPublishedEvent>
{
    private readonly string _phoneNumber;

    public SmsNotificationSubscriber(string phoneNumber)
    {
        _phoneNumber = phoneNumber;
    }

    public async Task HandleAsync(Events.NewsPublishedEvent @event)
    {
        // شبیه ‌سازی ارسال پیامک
        await Task.Delay(30); // شبیه ‌سازی تأخیر شبکه

        var shortMessage = @event.Title.Length > 50 
            ? @event.Title.Substring(0, 47) + "..." 
            : @event.Title;

        Console.WriteLine($"[SMS Notification] به {_phoneNumber} ارسال شد: {shortMessage}");
    }
}

