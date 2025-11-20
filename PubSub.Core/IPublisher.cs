namespace PubSub.Core;

/// <summary>
/// رابط برای ناشران (Publishers) که می‌توانند رویدادها را منتشر کنند
/// </summary>
public interface IPublisher
{
    /// <summary>
    /// انتشار یک رویداد به تمام مشترکان ثبت نام شده
    /// </summary>
    /// <typeparam name="TEvent">نوع رویداد</typeparam>
    /// <param name="event">رویداد برای انتشار</param>
    /// <returns>تسک برای انتظار کامل شدن عملیات انتشار</returns>
    Task PublishAsync<TEvent>(TEvent @event) where TEvent : IEvent;
}

