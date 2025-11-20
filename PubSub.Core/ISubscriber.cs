namespace PubSub.Core;

/// <summary>
/// رابط برای مشترکان (Subscribers) که می‌توانند به رویدادها گوش دهند
/// </summary>
/// <typeparam name="TEvent">نوع رویدادی که مشترک به آن گوش می‌دهد</typeparam>
public interface ISubscriber<in TEvent> where TEvent : IEvent
{
    /// <summary>
    /// متد callback که هنگام انتشار رویداد فراخوانی می‌شود
    /// </summary>
    /// <param name="event">رویداد منتشر شده</param>
    Task HandleAsync(TEvent @event);
}

