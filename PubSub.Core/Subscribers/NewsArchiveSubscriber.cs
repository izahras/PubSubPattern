namespace PubSub.Core.Subscribers;

/// <summary>
/// مشترک برای ذخیره ‌سازی خبر در آرشیو
/// </summary>
public class NewsArchiveSubscriber : ISubscriber<Events.NewsPublishedEvent>
{
    private readonly List<Events.NewsPublishedEvent> _archivedNews;

    public NewsArchiveSubscriber()
    {
        _archivedNews = new List<Events.NewsPublishedEvent>();
    }

    public async Task HandleAsync(Events.NewsPublishedEvent @event)
    {
        // شبیه ‌سازی ذخیره‌ سازی در دیتابیس
        await Task.Delay(20);

        lock (_archivedNews)
        {
            _archivedNews.Add(@event);
        }

        Console.WriteLine($"[Archive] خبر '{@event.Title}' در آرشیو ذخیره شد.");
    }

    /// <summary>
    /// دریافت تعداد خبرهای آرشیو شده
    /// </summary>
    public int GetArchivedCount()
    {
        lock (_archivedNews)
        {
            return _archivedNews.Count;
        }
    }

    /// <summary>
    /// دریافت لیست خبرهای آرشیو شده
    /// </summary>
    public IReadOnlyList<Events.NewsPublishedEvent> GetArchivedNews()
    {
        lock (_archivedNews)
        {
            return _archivedNews.ToList().AsReadOnly();
        }
    }
}

