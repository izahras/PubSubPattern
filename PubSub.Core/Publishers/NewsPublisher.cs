namespace PubSub.Core.Publishers;

/// <summary>
/// ناشر خبرها - مسئول انتشار رویدادهای مربوط به خبرها
/// </summary>
public class NewsPublisher
{
    private readonly IPublisher _publisher;

    public NewsPublisher(IPublisher publisher)
    {
        _publisher = publisher;
    }

    /// <summary>
    /// انتشار یک خبر جدید
    /// </summary>
    public async Task PublishNewsAsync(string title, string content, string category, string author)
    {
        var newsEvent = new Events.NewsPublishedEvent(title, content, category, author);
        await _publisher.PublishAsync(newsEvent);
    }

    /// <summary>
    /// به‌روزرسانی یک خبر موجود
    /// </summary>
    public async Task UpdateNewsAsync(Guid newsId, string? newTitle = null, string? newContent = null, bool isPublished = false)
    {
        var updateEvent = new Events.NewsUpdatedEvent(newsId, newTitle, newContent, isPublished);
        await _publisher.PublishAsync(updateEvent);
    }
}

