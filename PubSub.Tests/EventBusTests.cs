using PubSub.Core;
using PubSub.Core.Events;
using PubSub.Core.Subscribers;
using Xunit;

namespace PubSub.Tests;

/// <summary>
/// تست‌ های واحد برای EventBus
/// </summary>
public class EventBusTests
{
    [Fact]
    public void Subscribe_ShouldAddSubscriber()
    {
        // Arrange
        var eventBus = new EventBus();
        var subscriber = new TestSubscriber();

        // Act
        eventBus.Subscribe<NewsPublishedEvent>(subscriber);

        // Assert
        Assert.Equal(1, eventBus.GetSubscriberCount<NewsPublishedEvent>());
    }

    [Fact]
    public void Subscribe_WithSameSubscriberTwice_ShouldAddOnlyOnce()
    {
        // Arrange
        var eventBus = new EventBus();
        var subscriber = new TestSubscriber();

        // Act
        eventBus.Subscribe<NewsPublishedEvent>(subscriber);
        eventBus.Subscribe<NewsPublishedEvent>(subscriber);

        // Assert
        Assert.Equal(1, eventBus.GetSubscriberCount<NewsPublishedEvent>());
    }

    [Fact]
    public void Unsubscribe_ShouldRemoveSubscriber()
    {
        // Arrange
        var eventBus = new EventBus();
        var subscriber = new TestSubscriber();
        eventBus.Subscribe<NewsPublishedEvent>(subscriber);

        // Act
        eventBus.Unsubscribe<NewsPublishedEvent>(subscriber);

        // Assert
        Assert.Equal(0, eventBus.GetSubscriberCount<NewsPublishedEvent>());
    }

    [Fact]
    public async Task Publish_WithNoSubscribers_ShouldNotThrow()
    {
        // Arrange
        var eventBus = new EventBus();
        var newsEvent = new NewsPublishedEvent("Test", "Content", "Tech", "Author");

        // Act & Assert
        await eventBus.PublishAsync(newsEvent); // Should not throw
    }

    [Fact]
    public async Task Publish_ShouldNotifyAllSubscribers()
    {
        // Arrange
        var eventBus = new EventBus();
        var subscriber1 = new TestSubscriber();
        var subscriber2 = new TestSubscriber();
        var subscriber3 = new TestSubscriber();

        eventBus.Subscribe<NewsPublishedEvent>(subscriber1);
        eventBus.Subscribe<NewsPublishedEvent>(subscriber2);
        eventBus.Subscribe<NewsPublishedEvent>(subscriber3);

        var newsEvent = new NewsPublishedEvent("Test News", "Content", "Tech", "John Doe");

        // Act
        await eventBus.PublishAsync(newsEvent);

        // Assert
        Assert.True(subscriber1.ReceivedEvents.Count == 1, "Subscriber1 should receive event");
        Assert.True(subscriber2.ReceivedEvents.Count == 1, "Subscriber2 should receive event");
        Assert.True(subscriber3.ReceivedEvents.Count == 1, "Subscriber3 should receive event");

        Assert.Same(newsEvent, subscriber1.ReceivedEvents[0]);
        Assert.Same(newsEvent, subscriber2.ReceivedEvents[0]);
        Assert.Same(newsEvent, subscriber3.ReceivedEvents[0]);
    }

    [Fact]
    public async Task Publish_MultipleEvents_ShouldNotifySubscribersForEach()
    {
        // Arrange
        var eventBus = new EventBus();
        var subscriber = new TestSubscriber();
        eventBus.Subscribe<NewsPublishedEvent>(subscriber);

        var event1 = new NewsPublishedEvent("News 1", "Content 1", "Tech", "Author 1");
        var event2 = new NewsPublishedEvent("News 2", "Content 2", "Sports", "Author 2");
        var event3 = new NewsPublishedEvent("News 3", "Content 3", "Politics", "Author 3");

        // Act
        await eventBus.PublishAsync(event1);
        await eventBus.PublishAsync(event2);
        await eventBus.PublishAsync(event3);

        // Assert
        Assert.Equal(3, subscriber.ReceivedEvents.Count);
        Assert.Same(event1, subscriber.ReceivedEvents[0]);
        Assert.Same(event2, subscriber.ReceivedEvents[1]);
        Assert.Same(event3, subscriber.ReceivedEvents[2]);
    }

    [Fact]
    public async Task Publish_AfterUnsubscribe_ShouldNotNotifyUnsubscribedSubscriber()
    {
        // Arrange
        var eventBus = new EventBus();
        var subscriber1 = new TestSubscriber();
        var subscriber2 = new TestSubscriber();

        eventBus.Subscribe<NewsPublishedEvent>(subscriber1);
        eventBus.Subscribe<NewsPublishedEvent>(subscriber2);

        var event1 = new NewsPublishedEvent("News 1", "Content", "Tech", "Author");
        var event2 = new NewsPublishedEvent("News 2", "Content", "Tech", "Author");

        // Act
        await eventBus.PublishAsync(event1);
        eventBus.Unsubscribe<NewsPublishedEvent>(subscriber1);
        await eventBus.PublishAsync(event2);

        // Assert
        Assert.Single(subscriber1.ReceivedEvents); // فقط event1 را دریافت کرده
        Assert.Equal(2, subscriber2.ReceivedEvents.Count); // هر دو event را دریافت کرده
    }

    [Fact]
    public async Task Publish_DifferentEventTypes_ShouldOnlyNotifySubscribersOfThatType()
    {
        // Arrange
        var eventBus = new EventBus();
        var newsSubscriber = new TestSubscriber();
        var updateSubscriber = new TestUpdateSubscriber();

        eventBus.Subscribe<NewsPublishedEvent>(newsSubscriber);
        eventBus.Subscribe<NewsUpdatedEvent>(updateSubscriber);

        var newsEvent = new NewsPublishedEvent("News", "Content", "Tech", "Author");
        var updateEvent = new NewsUpdatedEvent(Guid.NewGuid(), "New Title", "New Content", true);

        // Act
        await eventBus.PublishAsync(newsEvent);
        await eventBus.PublishAsync(updateEvent);

        // Assert
        Assert.Single(newsSubscriber.ReceivedEvents);
        Assert.Single(updateSubscriber.ReceivedEvents);
    }

    [Fact]
    public void Subscribe_WithNullSubscriber_ShouldThrowArgumentNullException()
    {
        // Arrange
        var eventBus = new EventBus();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => eventBus.Subscribe<NewsPublishedEvent>(null!));
    }

    [Fact]
    public async Task Publish_WithNullEvent_ShouldThrowArgumentNullException()
    {
        // Arrange
        var eventBus = new EventBus();

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => 
            eventBus.PublishAsync<NewsPublishedEvent>(null!));
    }

    // Helper classes for testing
    private class TestSubscriber : ISubscriber<NewsPublishedEvent>
    {
        public List<NewsPublishedEvent> ReceivedEvents { get; } = new();

        public Task HandleAsync(NewsPublishedEvent @event)
        {
            ReceivedEvents.Add(@event);
            return Task.CompletedTask;
        }
    }

    private class TestUpdateSubscriber : ISubscriber<NewsUpdatedEvent>
    {
        public List<NewsUpdatedEvent> ReceivedEvents { get; } = new();

        public Task HandleAsync(NewsUpdatedEvent @event)
        {
            ReceivedEvents.Add(@event);
            return Task.CompletedTask;
        }
    }
}

