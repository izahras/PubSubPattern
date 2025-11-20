# پروژه پیاده‌سازی الگوی Publish-Subscribe 

این پروژه یک پیاده‌سازی کامل و قابل تست از الگوی **Publish-Subscribe (Pub-Sub)**  است. الگوی Pub-Sub یک الگوی طراحی رفتاری است که ارتباط بین ناشران (Publishers) و مشترکان (Subscribers) را از طریق یک واسط به نام Event Bus برقرار می‌کند.

## 📋 فهرست مطالب

- [معماری پروژه](#معماری-پروژه)
- [ساختار پروژه](#ساختار-پروژه)
- [پیش‌نیازها](#پیش‌نیازها)
- [نحوه اجرا](#نحوه-اجرا)
- [اجرای تست‌ها](#اجرای-تست‌ها)
- [توضیحات معماری](#توضیحات-معماری)
- [مثال‌های استفاده](#مثال‌های-استفاده)

## 🏗️ معماری پروژه

این پروژه شامل سه بخش اصلی است:

### 1. **PubSub.Core**
کتابخانه اصلی که شامل پیاده‌سازی هسته الگوی Pub-Sub است:

- **IEvent**: رابط پایه برای تمام رویدادها
- **ISubscriber<TEvent>**: رابط برای مشترکان که به رویدادها گوش می‌دهند
- **IPublisher**: رابط برای ناشران که رویدادها را منتشر می‌کنند
- **IEventBus**: رابط Event Bus که واسط بین Publisher و Subscriber ها است
- **EventBus**: پیاده‌سازی Event Bus با پشتیبانی از thread-safety

### 2. **PubSub.Example**
برنامه نمونه که نحوه استفاده از الگو را نشان می‌دهد با دامنه **سیستم انتشار خبر**.

### 3. **PubSub.Tests**
تست‌های واحد برای اطمینان از صحت پیاده‌سازی.

## 📁 ساختار پروژه

```
PUB-SUB/
├── PubSub.Core/                    # کتابخانه اصلی Pub-Sub
│   ├── IEvent.cs                   # رابط پایه رویدادها
│   ├── ISubscriber.cs              # رابط مشترکان
│   ├── IPublisher.cs               # رابط ناشران
│   ├── IEventBus.cs                # رابط Event Bus
│   ├── EventBus.cs                 # پیاده‌سازی Event Bus
│   ├── Events/                     # رویدادهای دامنه خبر
│   │   ├── NewsPublishedEvent.cs
│   │   └── NewsUpdatedEvent.cs
│   ├── Subscribers/                # مشترکان نمونه
│   │   ├── EmailNotificationSubscriber.cs
│   │   ├── SmsNotificationSubscriber.cs
│   │   ├── NewsArchiveSubscriber.cs
│   │   └── CategoryFilterSubscriber.cs
│   └── Publishers/                 # ناشران نمونه
│       └── NewsPublisher.cs
├── PubSub.Example/                 # برنامه نمونه
│   └── Program.cs
├── PubSub.Tests/                   # تست‌های واحد
│   ├── EventBusTests.cs
│   ├── SubscriberTests.cs
│   └── PublisherTests.cs
└── README.md
```

## 🔧 پیش‌نیازها

- **.NET SDK 8.0** یا بالاتر
- **Visual Studio 2022** یا **VS Code** یا هر IDE دیگری که از .NET پشتیبانی می‌کند

برای بررسی نسخه .NET نصب شده:
```bash
dotnet --version
```

## 🚀 نحوه اجرا

### 1. کلون یا دانلود پروژه

```bash
cd PUB-SUB
```

### 2. Restore پکیج‌ها

```bash
dotnet restore
```

### 3. Build پروژه

```bash
dotnet build
```

### 4. اجرای برنامه نمونه

```bash
dotnet run --project PubSub.Example/PubSub.Example.csproj
```

## 🧪 اجرای تست‌ها

برای اجرای تمام تست‌ها:

```bash
dotnet test
```

## 📐 توضیحات معماری

### الگوی Publish-Subscribe

الگوی Pub-Sub یک الگوی طراحی است که ارتباط بین کامپوننت‌ها را از طریق یک واسط (Event Bus) برقرار می‌کند. این الگو مزایای زیر را دارد:

1. **جداسازی (Decoupling)**: Publisher و Subscriber هیچ اطلاعی از یکدیگر ندارند
2. **قابلیت گسترش**: می‌توان Subscriber های جدید را بدون تغییر در Publisher اضافه کرد
3. **ارتباط ناهمزمان**: رویدادها به صورت ناهمزمان پردازش می‌شوند

### جریان کار

```
┌─────────────┐
│  Publisher  │
└──────┬──────┘
       │ Publish
       ▼
┌─────────────┐
│  Event Bus  │
└──────┬──────┘
       │ Notify
       ├─────────┬─────────┐
       ▼         ▼         ▼
  ┌────────┐ ┌────────┐ ┌────────┐
  │ Sub 1  │ │ Sub 2  │ │ Sub 3  │
  └────────┘ └────────┘ └────────┘
```

### ویژگی‌های پیاده‌سازی

- ✅ **Thread-Safe**: استفاده از `ConcurrentDictionary` و lock برای thread-safety
- ✅ **Type-Safe**: استفاده از Generic Types برای type safety
- ✅ **Error Handling**: مدیریت خطا برای جلوگیری از تأثیر خطای یک Subscriber روی بقیه
- ✅ **Asynchronous**: پشتیبانی کامل از async/await
- ✅ **Extensible**: قابلیت توسعه آسان

## 💡 مثال‌های استفاده

### مثال 1: ثبت‌نام Subscriber و انتشار رویداد

```csharp
// ایجاد EventBus
var eventBus = new EventBus();

// ایجاد و ثبت Subscriber
var emailSubscriber = new EmailNotificationSubscriber("user@example.com");
eventBus.Subscribe<NewsPublishedEvent>(emailSubscriber);

// ایجاد Publisher
var newsPublisher = new NewsPublisher(eventBus);

// انتشار رویداد
await newsPublisher.PublishNewsAsync(
    title: "خبر جدید",
    content: "محتوای خبر",
    category: "تکنولوژی",
    author: "نویسنده"
);
```

### مثال 2: چند Subscriber برای یک رویداد

```csharp
var eventBus = new EventBus();

// ثبت چند Subscriber
eventBus.Subscribe<NewsPublishedEvent>(new EmailNotificationSubscriber("email@example.com"));
eventBus.Subscribe<NewsPublishedEvent>(new SmsNotificationSubscriber("09123456789"));
eventBus.Subscribe<NewsPublishedEvent>(new NewsArchiveSubscriber());

var publisher = new NewsPublisher(eventBus);
await publisher.PublishNewsAsync("عنوان", "محتوا", "دسته", "نویسنده");
// هر سه Subscriber رویداد را دریافت می‌کنند
```

### مثال 3: فیلتر بر اساس دسته‌بندی

```csharp
var eventBus = new EventBus();

var techSubscriber = new EmailNotificationSubscriber("tech@example.com");
var categoryFilter = new CategoryFilterSubscriber("تکنولوژی", techSubscriber);

eventBus.Subscribe<NewsPublishedEvent>(categoryFilter);

var publisher = new NewsPublisher(eventBus);

// فقط این رویداد ارسال می‌شود (دسته‌بندی تکنولوژی)
await publisher.PublishNewsAsync("AI News", "Content", "تکنولوژی", "Author");

// این رویداد ارسال نمی‌شود (دسته‌بندی ورزشی)
await publisher.PublishNewsAsync("Sports News", "Content", "ورزشی", "Author");
```

### مثال 4: لغو ثبت‌نام

```csharp
var eventBus = new EventBus();
var subscriber = new EmailNotificationSubscriber("user@example.com");

eventBus.Subscribe<NewsPublishedEvent>(subscriber);
// ... انتشار رویدادها

// لغو ثبت‌نام
eventBus.Unsubscribe<NewsPublishedEvent>(subscriber);
// از این پس این Subscriber رویدادها را دریافت نمی‌کند
```

## 📝 یادداشت‌های طراحی

1. **Thread-Safety**: `EventBus` از `ConcurrentDictionary` استفاده می‌کند و برای عملیات‌های لیست از lock استفاده می‌شود.

2. **Error Isolation**: اگر یک Subscriber خطا دهد، خطای آن روی Subscriber های دیگر تأثیر نمی‌گذارد.

3. **Asynchronous Processing**: تمام Subscriber ها به صورت همزمان (parallel) رویدادها را دریافت می‌کنند.

4. **Type Safety**: استفاده از Generic Types باعث می‌شود فقط Subscriber های مناسب برای هر نوع رویداد ثبت‌نام شوند.

## 🔍 تست‌ها

پروژه شامل تست‌های جامعی است که موارد زیر را پوشش می‌دهند:

- ✅ ثبت‌نام و لغو ثبت‌نام Subscriber ها
- ✅ انتشار رویداد به چند Subscriber
- ✅ انتشار رویدادهای مختلف
- ✅ فیلتر کردن رویدادها
- ✅ مدیریت خطاها
- ✅ Thread-safety


**نکته**: این پیاده‌سازی تمرکز بر **الگوی Pub-Sub** دارد و از فریمورک‌های خاص استفاده نمی‌کند. برای استفاده در محیط‌های production، ممکن است نیاز به افزودن قابلیت‌هایی مانند logging، retry mechanism، و persistence باشد.

