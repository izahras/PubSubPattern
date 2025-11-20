namespace PubSub.Core.Subscribers;

/// <summary>
/// مشترک برای فیلتر کردن خبرها بر اساس دسته‌بندی
/// فقط به خبرهای دسته‌بندی مشخص شده گوش می‌دهد
/// </summary>
public class CategoryFilterSubscriber : ISubscriber<Events.NewsPublishedEvent>
{
    private readonly string _category;
    private readonly ISubscriber<Events.NewsPublishedEvent> _innerSubscriber;

    public CategoryFilterSubscriber(string category, ISubscriber<Events.NewsPublishedEvent> innerSubscriber)
    {
        _category = category ;
        _innerSubscriber = innerSubscriber ;
    }

    public async Task HandleAsync(Events.NewsPublishedEvent @event)
    {
        // فقط اگر دسته‌بندی مطابقت داشت به subscriber داخلی ارسال می‌کند
        if (string.Equals(@event.Category, _category, StringComparison.OrdinalIgnoreCase))
        {
            await _innerSubscriber.HandleAsync(@event);
        }
    }
}

