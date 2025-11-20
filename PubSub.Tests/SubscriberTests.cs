using PubSub.Core;
using PubSub.Core.Events;
using PubSub.Core.Subscribers;
using Xunit;

namespace PubSub.Tests;

/// <summary>
/// تست‌های واحد برای Subscriber ها
/// </summary>
public class SubscriberTests
{
    [Fact]
    public async Task NewsArchiveSubscriber_ShouldArchiveNews()
    {
        // Arrange
        var archiveSubscriber = new NewsArchiveSubscriber();
        var newsEvent = new NewsPublishedEvent("Test News", "Content", "Tech", "Author");

        // Act
        await archiveSubscriber.HandleAsync(newsEvent);

        // Assert
        Assert.Equal(1, archiveSubscriber.GetArchivedCount());
        var archivedNews = archiveSubscriber.GetArchivedNews();
        Assert.Single(archivedNews);
        Assert.Same(newsEvent, archivedNews[0]);
    }

    [Fact]
    public async Task NewsArchiveSubscriber_MultipleNews_ShouldArchiveAll()
    {
        // Arrange
        var archiveSubscriber = new NewsArchiveSubscriber();
        var event1 = new NewsPublishedEvent("News 1", "Content 1", "Tech", "Author 1");
        var event2 = new NewsPublishedEvent("News 2", "Content 2", "Sports", "Author 2");

        // Act
        await archiveSubscriber.HandleAsync(event1);
        await archiveSubscriber.HandleAsync(event2);

        // Assert
        Assert.Equal(2, archiveSubscriber.GetArchivedCount());
        var archivedNews = archiveSubscriber.GetArchivedNews();
        Assert.Equal(2, archivedNews.Count);
    }

    [Fact]
    public async Task CategoryFilterSubscriber_ShouldFilterByCategory()
    {
        // Arrange
        var innerSubscriber = new TestSubscriber();
        var filterSubscriber = new CategoryFilterSubscriber("Tech", innerSubscriber);

        var techNews = new NewsPublishedEvent("Tech News", "Content", "Tech", "Author");
        var sportsNews = new NewsPublishedEvent("Sports News", "Content", "Sports", "Author");

        // Act
        await filterSubscriber.HandleAsync(techNews);
        await filterSubscriber.HandleAsync(sportsNews);

        // Assert
        Assert.Single(innerSubscriber.ReceivedEvents);
        Assert.Same(techNews, innerSubscriber.ReceivedEvents[0]);
    }

    [Fact]
    public async Task CategoryFilterSubscriber_CaseInsensitive_ShouldMatch()
    {
        // Arrange
        var innerSubscriber = new TestSubscriber();
        var filterSubscriber = new CategoryFilterSubscriber("tech", innerSubscriber);

        var techNews = new NewsPublishedEvent("Tech News", "Content", "TECH", "Author");

        // Act
        await filterSubscriber.HandleAsync(techNews);

        // Assert
        Assert.Single(innerSubscriber.ReceivedEvents);
    }

    // Helper class for testing
    private class TestSubscriber : ISubscriber<NewsPublishedEvent>
    {
        public List<NewsPublishedEvent> ReceivedEvents { get; } = new();

        public Task HandleAsync(NewsPublishedEvent @event)
        {
            ReceivedEvents.Add(@event);
            return Task.CompletedTask;
        }
    }
}

