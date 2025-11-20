namespace PubSub.Core;

/// <summary>
/// رابط پایه برای تمام رویدادها در سیستم Pub-Sub
/// </summary>
public interface IEvent
{
    /// <summary>
    /// شناسه یکتا برای رویداد
    /// </summary>
    Guid EventId { get; }

    /// <summary>
    /// زمان ایجاد رویداد
    /// </summary>
    DateTime Timestamp { get; }
}

