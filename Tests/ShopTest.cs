using Core;
using Core.Articles;
using Core.Orders;
using Core.Sales;

using FluentAssertions;

using JetBrains.Annotations;

using Moq;

namespace Tests;

[TestSubject(typeof(Shop))]
public class ShopTest
{
    private readonly Mock<IRepository> _repository = new();
    private readonly Shop _service;

    public ShopTest()
    {
        _repository.Setup(r => r.Save(It.IsAny<SoldArticle>())).Returns(Unit.Value);
        _service = new Shop(_repository.Object, Mock.Of<ILogger>(), []);
    }

    [Fact]
    public void OrderAndSellArticle_SavesArticle()
    {
        Mock<IOrder> order = new();
        Article article = new(1, "", 100);
        order
            .Setup(o => o.OrderArticle(It.IsAny<IEnumerable<ISupplier>>()))
            .Returns(article);

        Mock<ISale> sale = new();
        SoldArticle soldArticle = new(article, DateTimeOffset.UtcNow, 1);
        sale.Setup(s => s.SellArticle(It.IsAny<Article>()))
            .Returns(soldArticle);


        _service.OrderAndSellArticle(order.Object, sale.Object).IsSuccess.Should().BeTrue();
        _repository.Verify(r => r.Save(It.Is<SoldArticle>(a => soldArticle == a)), Times.Once);
    }

    [Fact]
    public void GetById_ReturnsArticlesFromRepository()
    {
        _service.GetSoldArticles(0);
        _repository.Verify(r => r.Query(It.IsAny<int>()), Times.Once);
    }
}