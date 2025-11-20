using PubSub.Core;
using PubSub.Core.Events;
using PubSub.Core.Publishers;
using PubSub.Core.Subscribers;

namespace PubSub.Example;

/// <summary>
/// برنامه نمونه برای نمایش کارکرد الگوی Publish-Subscribe
/// </summary>
class Program
{
    static async Task Main(string[] args)
    {
        // تنظیم encoding برای نمایش صحیح کاراکترهای فارسی در Console
        try
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;
        }
        catch
        {
            // در صورت عدم پشتیبانی از UTF-8، از encoding پیش‌فرض استفاده می‌شود
        }
        
        Console.WriteLine("====================================");
        Console.WriteLine("نمونه عملی الگوی Publish-Subscribe");
        Console.WriteLine("سیستم انتشار خبر");
        Console.WriteLine("====================================\n");

        var eventBus = new EventBus();

        var emailSubscriber = new EmailNotificationSubscriber("user@example.com");
        var smsSubscriber = new SmsNotificationSubscriber("09123456789");
        var archiveSubscriber = new NewsArchiveSubscriber();

        eventBus.Subscribe<NewsPublishedEvent>(emailSubscriber);
        eventBus.Subscribe<NewsPublishedEvent>(smsSubscriber);
        eventBus.Subscribe<NewsPublishedEvent>(archiveSubscriber);

        Console.WriteLine($"تعداد مشترکان ثبت نام شده: {eventBus.GetSubscriberCount<NewsPublishedEvent>()}\n");

        // ایجاد Publisher
        var newsPublisher = new NewsPublisher(eventBus);

        // انتشار خبرها
        Console.WriteLine("--- انتشار خبر 1 ---");
        await newsPublisher.PublishNewsAsync(
            title: "آخرین پیشرفت‌های هوش مصنوعی در سال 2024",
            content: "هوش مصنوعی در سال جاری پیشرفت‌های چشمگیری داشته است...",
            category: "تکنولوژی",
            author: "میثم احمدی"
        );

        await Task.Delay(500); // فاصله بین خبرها

        Console.WriteLine("\n--- انتشار خبر 2 ---");
        await newsPublisher.PublishNewsAsync(
            title: "برگزاری مسابقات جام جهانی 2024",
            content: "مسابقات جام جهانی در کشور میزبان آغاز شد...",
            category: "ورزشی",
            author: "سارا رضایی"
        );

        await Task.Delay(500);

        Console.WriteLine("\n--- انتشار خبر 3 ---");
        await newsPublisher.PublishNewsAsync(
            title: "سیاست جدید دولت در زمینه انرژی",
            content: "دولت سیاست جدیدی برای مدیریت انرژی اعلام کرد...",
            category: "سیاسی",
            author: "محمد کریمی"
        );

        await Task.Delay(500);

        // نمایش آمار
        Console.WriteLine($"\n--- آمار آرشیو ---");
        Console.WriteLine($"تعداد خبرهای آرشیو شده: {archiveSubscriber.GetArchivedCount()}");

        // مثال فیلتر بر اساس دسته‌بندی
        Console.WriteLine("\n====================================");
        Console.WriteLine("مثال: فیلتر بر اساس دسته‌بندی");
        Console.WriteLine("====================================\n");

        var techEmailSubscriber = new EmailNotificationSubscriber("tech@example.com");
        var categoryFilter = new CategoryFilterSubscriber("تکنولوژی", techEmailSubscriber);

        eventBus.Subscribe<NewsPublishedEvent>(categoryFilter);

        Console.WriteLine("--- انتشار خبر جدید تکنولوژی ---");
        await newsPublisher.PublishNewsAsync(
            title: "راه‌اندازی سیستم‌های جدید پردازش ابری",
            content: "شرکت‌های بزرگ فناوری سیستم‌های جدیدی راه‌اندازی کردند...",
            category: "تکنولوژی",
            author: "زهرا صادقی"
        );

        await Task.Delay(200);

        Console.WriteLine("\n--- انتشار خبر جدید ورزشی ---");
        await newsPublisher.PublishNewsAsync(
            title: "نتایج مسابقات فوتبال",
            content: "نتایج آخرین مسابقات فوتبال اعلام شد...",
            category: "ورزشی",
            author: "علیرضا زارع"
        );

        Console.WriteLine("\n--- خاتمه برنامه ---");
        Console.WriteLine("نمونه عملی با موفقیت اجرا شد!");
        Console.ReadKey();
    }
}
