using FluentAssertions;

using JetBrains.Annotations;

using Moq;

using Core;
using Core.Articles;
using Core.Common;
using Core.Sales;

namespace Tests;

[TestSubject(typeof(BudgetSale))]
public class BudgetSaleTest
{
    [Fact]
    public void SellArticle_EnoughMoney_ReturnsSoldArticle()
    {
        Article article = new(0, String.Empty, 10);
        Mock<TimeProvider> time = new();
        DateTimeOffset now = DateTimeOffset.UtcNow;
        time.Setup(t => t.GetUtcNow()).Returns(now);
        const int buyerId = 1;

        new BudgetSale(buyerId, 10, time.Object)
            .SellArticle(article)
            .Switch(val =>
            {
                val.Details.Should().Be(article);
                val.Timestamp.Should().Be(now);
                val.BuyerId.Should().Be(buyerId);
            }, Assert.Fail);
    }

    [Fact]
    public void SellArticle_NotEnoughMoney_Fails()
    {
        BudgetSale sale = new(1, 10, Mock.Of<TimeProvider>());
        sale.SellArticle(new Article(0, String.Empty, 5)).IsSuccess.Should().BeTrue();
        sale.SellArticle(new Article(0, String.Empty, 10)).IsFailure.Should().BeTrue();
    }
}