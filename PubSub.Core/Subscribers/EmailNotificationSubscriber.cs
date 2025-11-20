namespace PubSub.Core.Subscribers;

/// <summary>
/// مشترک برای ارسال اطلاعیه ایمیلی هنگام انتشار خبر جدید
/// </summary>
public class EmailNotificationSubscriber : ISubscriber<Events.NewsPublishedEvent>
{
    private readonly string _recipientEmail;

    public EmailNotificationSubscriber(string recipientEmail)
    {
        _recipientEmail = recipientEmail ;
    }

    public async Task HandleAsync(Events.NewsPublishedEvent @event)
    {
        // شبیه ‌سازی ارسال ایمیل
        await Task.Delay(50); // شبیه ‌سازی تأخیر شبکه

        Console.WriteLine($"[Email Notification] به {_recipientEmail} ارسال شد: {@event.Title}");
        Console.WriteLine($"    دسته‌ بندی: {@event.Category}");
        Console.WriteLine($"    نویسنده: {@event.Author}");
    }
}

