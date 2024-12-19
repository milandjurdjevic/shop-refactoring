using Core;
using Core.Articles;
using Core.Common;
using Core.Orders;

using FluentAssertions;

using JetBrains.Annotations;

using Moq;

namespace Tests;

[TestSubject(typeof(BestPriceOrder))]
public class BestPriceOrderTest
{
    private readonly Mock<ISupplier> _supplier1 = new();
    private readonly Mock<ISupplier> _supplier2 = new();
    private readonly Mock<ISupplier> _supplier3 = new();

    private readonly ISupplier[] _suppliers;

    public BestPriceOrderTest()
    {
        _suppliers = [_supplier1.Object, _supplier2.Object, _supplier3.Object];
    }

    [Fact]
    public void OrderArticle_ArticleUnderThreshold_ReturnsArticle()
    {
        _supplier1.Setup(s => s.ArticleInInventory(It.IsAny<int>())).Returns(true);
        _supplier1.Setup(s => s.GetArticle(It.IsAny<int>())).Returns(new Article(1, "", 100));

        _supplier2.Setup(s => s.ArticleInInventory(It.IsAny<int>())).Returns(true);
        _supplier2.Setup(s => s.GetArticle(It.IsAny<int>())).Returns(new Article(1, "", 90));

        new BestPriceOrder(0, 99)
            .OrderArticle(_suppliers)
            .Switch(val => val.Price.Should().Be(90), Assert.Fail);
    }

    [Fact]
    public void OrderArticle_SeveralArticlesUnderThreshold_ReturnsArticleWithTheLowestPrice()
    {
        _supplier1.Setup(s => s.ArticleInInventory(It.IsAny<int>())).Returns(true);
        _supplier1.Setup(s => s.GetArticle(It.IsAny<int>())).Returns(new Article(1, "", 100));

        _supplier2.Setup(s => s.ArticleInInventory(It.IsAny<int>())).Returns(true);
        _supplier2.Setup(s => s.GetArticle(It.IsAny<int>())).Returns(new Article(1, "", 90));

        _supplier3.Setup(s => s.ArticleInInventory(It.IsAny<int>())).Returns(true);
        _supplier3.Setup(s => s.GetArticle(It.IsAny<int>())).Returns(new Article(1, "", 80));

        new BestPriceOrder(0, 95)
            .OrderArticle(_suppliers)
            .Switch(val => val.Price.Should().Be(80), Assert.Fail);
    }

    [Fact]
    public void OrderArticle_NoArticlesUnderThreshold_Fails()
    {
        _supplier1.Setup(s => s.ArticleInInventory(It.IsAny<int>())).Returns(true);
        _supplier1.Setup(s => s.GetArticle(It.IsAny<int>())).Returns(new Article(1, "", 100));

        _supplier2.Setup(s => s.ArticleInInventory(It.IsAny<int>())).Returns(true);
        _supplier2.Setup(s => s.GetArticle(It.IsAny<int>())).Returns(new Article(1, "", 90));

        _supplier3.Setup(s => s.ArticleInInventory(It.IsAny<int>())).Returns(true);
        _supplier3.Setup(s => s.GetArticle(It.IsAny<int>())).Returns(new Article(1, "", 80));

        new BestPriceOrder(0, 75)
            .OrderArticle(_suppliers)
            .IsFailure
            .Should()
            .BeTrue();
    }
}