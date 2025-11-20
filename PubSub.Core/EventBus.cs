using System.Collections.Concurrent;

namespace PubSub.Core;

/// <summary>
/// پیاده ‌سازی Event Bus برای الگوی Publish-Subscribe
/// این کلاس واسط بین Publisher و Subscriber ها است
/// </summary>
public class EventBus : IEventBus
{
    // استفاده از ConcurrentDictionary برای thread-safety
    private readonly ConcurrentDictionary<Type, List<object>> _subscribers;

    /// <summary>
    /// سازنده EventBus
    /// </summary>
    public EventBus()
    {
        _subscribers = new ConcurrentDictionary<Type, List<object>>();
    }

    /// <summary>
    /// ثبت ‌نام یک مشترک برای دریافت رویدادها از نوع مشخص
    /// </summary>
    public void Subscribe<TEvent>(ISubscriber<TEvent> subscriber) where TEvent : IEvent
    {
        if (subscriber == null)
            throw new ArgumentNullException(nameof(subscriber));

        var eventType = typeof(TEvent);
        var subscribers = _subscribers.GetOrAdd(eventType, _ => new List<object>());

        lock (subscribers)
        {
            if (!subscribers.Contains(subscriber))
            {
                subscribers.Add(subscriber);
            }
        }
    }

    /// <summary>
    /// لغو ثبت‌ نام یک مشترک
    /// </summary>
    public void Unsubscribe<TEvent>(ISubscriber<TEvent> subscriber) where TEvent : IEvent
    {
        if (subscriber == null)
            return;

        var eventType = typeof(TEvent);
        if (_subscribers.TryGetValue(eventType, out var subscribers))
        {
            lock (subscribers)
            {
                subscribers.Remove(subscriber);
            }
        }
    }

    /// <summary>
    /// انتشار یک رویداد به تمام مشترکان ثبت‌ نام شده
    /// </summary>
    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent
    {
        if (@event == null)
            throw new ArgumentNullException(nameof(@event));

        var eventType = typeof(TEvent);
        if (!_subscribers.TryGetValue(eventType, out var subscribers))
        {
            // هیچ مشترکی ثبت‌ نام نشده است
            return;
        }

        // ایجاد کپی از لیست برای جلوگیری از تغییرات همزمان
        List<ISubscriber<TEvent>> subscribersCopy;
        lock (subscribers)
        {
            subscribersCopy = subscribers
                .OfType<ISubscriber<TEvent>>()
                .ToList();
        }

        // ارسال رویداد به همه مشترکان به صورت همزمان
        var tasks = subscribersCopy.Select(subscriber => 
            Task.Run(async () =>
            {
                try
                {
                    await subscriber.HandleAsync(@event);
                }
                catch (Exception ex)
                {
                    // لاگ خطا در صورت نیاز - در اینجا فقط مانع از انتشار خطا می‌شویم
                    // تا خطای یک subscriber دیگر subscriber ها را متأثر نکند
                    OnSubscriberError(subscriber, @event, ex);
                }
            })
        );

        await Task.WhenAll(tasks);
    }

    /// <summary>
    /// متد callback برای مدیریت خطاهای subscriber ها
    /// می‌تواند override شود برای لاگ کردن یا مدیریت خطا
    /// </summary>
    protected virtual void OnSubscriberError<TEvent>(ISubscriber<TEvent> subscriber, TEvent @event, Exception exception)
        where TEvent : IEvent
    {
        // در پیاده‌ سازی واقعی می‌توان اینجا لاگ کرد
    }

    /// <summary>
    /// دریافت تعداد مشترکان برای یک نوع رویداد خاص
    /// </summary>
    public int GetSubscriberCount<TEvent>() where TEvent : IEvent
    {
        var eventType = typeof(TEvent);
        if (_subscribers.TryGetValue(eventType, out var subscribers))
        {
            lock (subscribers)
            {
                return subscribers.Count;
            }
        }
        return 0;
    }
}

