using FluentAssertions;

using JetBrains.Annotations;

using Moq;

using Shop.Core;

namespace Shop.Tests;

[TestSubject(typeof(DefaultOrder))]
public class DefaultOrderTest
{
    private readonly Mock<ISupplier> _supplier1 = new();
    private readonly Mock<ISupplier> _supplier2 = new();
    private readonly Mock<ISupplier> _supplier3 = new();

    private Result<Article, string> OrderArticle(int price)
    {
        return new DefaultOrder(0, price).OrderArticle([_supplier1.Object, _supplier2.Object, _supplier3.Object]);
    }

    [Fact]
    public void OrderArticle_ReturnsFirstArticleBelowThreshold()
    {
        _supplier1.Setup(s => s.ArticleInInventory(It.IsAny<int>())).Returns(true);
        _supplier1.Setup(s => s.GetArticle(It.IsAny<int>())).Returns(new Article(1, "", 100));
        _supplier2.Setup(s => s.ArticleInInventory(It.IsAny<int>())).Returns(true);
        _supplier2.Setup(s => s.GetArticle(It.IsAny<int>())).Returns(new Article(1, "", 90));
        _supplier3.Setup(s => s.ArticleInInventory(It.IsAny<int>())).Returns(true);
        _supplier3.Setup(s => s.GetArticle(It.IsAny<int>())).Returns(new Article(1, "", 80));
        OrderArticle(95).Switch(val => val.Price.Should().Be(90), Assert.Fail);
    }

    [Fact]
    public void OrderArticle_NoArticleInInventory_ReturnsTheLastArticleInInventory()
    {
        _supplier1.Setup(s => s.ArticleInInventory(It.IsAny<int>())).Returns(true);
        _supplier1.Setup(s => s.GetArticle(It.IsAny<int>())).Returns(new Article(1, "", 90));
        _supplier2.Setup(s => s.ArticleInInventory(It.IsAny<int>())).Returns(false);
        _supplier3.Setup(s => s.ArticleInInventory(It.IsAny<int>())).Returns(true);
        OrderArticle(85).Switch(val => val.Price.Should().Be(90), Assert.Fail);
    }

    [Fact]
    public void OrderArticle_NoArticleBelowThreshold_ReturnsLastArticle()
    {
        _supplier1.Setup(s => s.ArticleInInventory(It.IsAny<int>())).Returns(true);
        _supplier1.Setup(s => s.GetArticle(It.IsAny<int>())).Returns(new Article(1, "", 100));
        _supplier2.Setup(s => s.ArticleInInventory(It.IsAny<int>())).Returns(true);
        _supplier2.Setup(s => s.GetArticle(It.IsAny<int>())).Returns(new Article(1, "", 80));
        _supplier3.Setup(s => s.ArticleInInventory(It.IsAny<int>())).Returns(true);
        _supplier3.Setup(s => s.GetArticle(It.IsAny<int>())).Returns(new Article(1, "", 90));
        OrderArticle(70).Switch(val => val.Price.Should().Be(90), Assert.Fail);
    }

    [Fact]
    public void OrderArticle_ArticleNotInInventoryAtFirstSupplier_ReturnsError()
    {
        _supplier1.Setup(s => s.ArticleInInventory(It.IsAny<int>())).Returns(false);
        _supplier1.Setup(s => s.GetArticle(It.IsAny<int>())).Returns(new Article(1, "", 100));
        _supplier2.Setup(s => s.ArticleInInventory(It.IsAny<int>())).Returns(true);
        _supplier2.Setup(s => s.GetArticle(It.IsAny<int>())).Returns(new Article(1, "", 80));
        _supplier3.Setup(s => s.ArticleInInventory(It.IsAny<int>())).Returns(true);
        _supplier3.Setup(s => s.GetArticle(It.IsAny<int>())).Returns(new Article(1, "", 90));
        OrderArticle(100).IsFailure.Should().BeTrue();
    }

    [Fact]
    public void OrderArticle_ArticleNotFound_ReturnsError()
    {
        OrderArticle(0).IsFailure.Should().BeTrue();
    }
}