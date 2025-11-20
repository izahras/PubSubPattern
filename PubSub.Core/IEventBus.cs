namespace PubSub.Core;

/// <summary>
/// رابط Event Bus که نقش واسط بین Publisher و Subscriber ها را دارد
/// </summary>
public interface IEventBus : IPublisher
{
    /// <summary>
    /// ثبت نام یک مشترک برای دریافت رویدادها
    /// </summary>
    /// <typeparam name="TEvent">نوع رویداد</typeparam>
    /// <param name="subscriber">مشترک برای ثبت نام</param>
    void Subscribe<TEvent>(ISubscriber<TEvent> subscriber) where TEvent : IEvent;

    /// <summary>
    /// لغو ثبت نام یک مشترک
    /// </summary>
    /// <typeparam name="TEvent">نوع رویداد</typeparam>
    /// <param name="subscriber">مشترک برای لغو ثبت نام</param>
    void Unsubscribe<TEvent>(ISubscriber<TEvent> subscriber) where TEvent : IEvent;
}

