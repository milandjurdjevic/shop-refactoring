using FluentAssertions;

using JetBrains.Annotations;

using Moq;

using Core;
using Core.Articles;
using Core.Sales;

namespace Tests;

[TestSubject(typeof(DefaultSale))]
public class DefaultSaleTest
{
    [Fact]
    public void SellArticle_ReturnsSoldArticle()
    {
        Article article = new(1, "Test", 100);
        DateTimeOffset now = DateTimeOffset.UtcNow;
        Mock<TimeProvider> time = new();
        time.Setup(t => t.GetUtcNow()).Returns(now);
        const int buyerId = 10;

        new DefaultSale(buyerId, time.Object)
            .SellArticle(article)
            .Switch(val =>
            {
                val.Details.Should().Be(article);
                val.BuyerId.Should().Be(buyerId);
                val.Timestamp.Should().Be(now);
            }, Assert.Fail);
    }
}