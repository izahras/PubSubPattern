namespace PubSub.Core.Events;

/// <summary>
/// رویداد انتشار خبر جدید
/// </summary>
public class NewsPublishedEvent : IEvent
{
    public Guid EventId { get; } = Guid.NewGuid();
    public DateTime Timestamp { get; } = DateTime.UtcNow;

    /// <summary>
    /// عنوان خبر
    /// </summary>
    public string Title { get; }

    /// <summary>
    /// محتوای خبر
    /// </summary>
    public string Content { get; }

    /// <summary>
    /// دسته‌بندی خبر
    /// </summary>
    public string Category { get; }

    /// <summary>
    /// نویسنده خبر
    /// </summary>
    public string Author { get; }

    public NewsPublishedEvent(string title, string content, string category, string author)
    {
        Title = title;
        Content = content;
        Category = category;
        Author = author;
    }

    public override string ToString()
    {
        return $"[{Category}] {Title} - by {Author}";
    }
}

