using PubSub.Core;
using PubSub.Core.Events;
using PubSub.Core.Publishers;
using PubSub.Core.Subscribers;
using Xunit;

namespace PubSub.Tests;

/// <summary>
/// تست‌های واحد برای Publisher
/// </summary>
public class PublisherTests
{
    [Fact]
    public async Task NewsPublisher_PublishNews_ShouldPublishEvent()
    {
        // Arrange
        var eventBus = new EventBus();
        var newsPublisher = new NewsPublisher(eventBus);
        var subscriber = new TestSubscriber();
        eventBus.Subscribe<NewsPublishedEvent>(subscriber);

        // Act
        await newsPublisher.PublishNewsAsync("Test Title", "Test Content", "Tech", "John Doe");

        // Assert
        Assert.Single(subscriber.ReceivedEvents);
        var receivedEvent = subscriber.ReceivedEvents[0];
        Assert.Equal("Test Title", receivedEvent.Title);
        Assert.Equal("Test Content", receivedEvent.Content);
        Assert.Equal("Tech", receivedEvent.Category);
        Assert.Equal("John Doe", receivedEvent.Author);
    }

    [Fact]
    public async Task NewsPublisher_UpdateNews_ShouldPublishUpdateEvent()
    {
        // Arrange
        var eventBus = new EventBus();
        var newsPublisher = new NewsPublisher(eventBus);
        var subscriber = new TestUpdateSubscriber();
        eventBus.Subscribe<NewsUpdatedEvent>(subscriber);

        var newsId = Guid.NewGuid();

        // Act
        await newsPublisher.UpdateNewsAsync(newsId, "New Title", "New Content", isPublished: true);

        // Assert
        Assert.Single(subscriber.ReceivedEvents);
        var receivedEvent = subscriber.ReceivedEvents[0];
        Assert.Equal(newsId, receivedEvent.NewsId);
        Assert.Equal("New Title", receivedEvent.NewTitle);
        Assert.Equal("New Content", receivedEvent.NewContent);
        Assert.True(receivedEvent.IsPublished);
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

