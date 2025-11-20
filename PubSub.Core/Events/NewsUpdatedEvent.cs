namespace PubSub.Core.Events;

/// <summary>
/// رویداد به‌روزرسانی خبر موجود
/// </summary>
public class NewsUpdatedEvent : IEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime Timestamp { get; } = DateTime.UtcNow;

    /// <summary>
    /// شناسه خبر
    /// </summary>
    public Guid NewsId { get; }

    /// <summary>
    /// عنوان جدید
    /// </summary>
    public string? NewTitle { get; }

    /// <summary>
    /// محتوای جدید
    /// </summary>
    public string? NewContent { get; }

    /// <summary>
    /// آیا خبر منتشر شده است؟
    /// </summary>
    public bool IsPublished { get; }

    public NewsUpdatedEvent(Guid newsId, string? newTitle = null, string? newContent = null, bool isPublished = false)
    {
        NewsId = newsId;
        NewTitle = newTitle;
        NewContent = newContent;
        IsPublished = isPublished;
    }
}

