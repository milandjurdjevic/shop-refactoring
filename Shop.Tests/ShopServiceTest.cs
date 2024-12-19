using FluentAssertions;

using JetBrains.Annotations;

using Moq;

using Shop.Core;

namespace Shop.Tests;

[TestSubject(typeof(ShopService))]
public class ShopServiceTest
{
    private readonly Mock<IRepository> _repository = new();
    private readonly ShopService _service;

    public ShopServiceTest()
    {
        _repository.Setup(r => r.Save(It.IsAny<SoldArticle>())).Returns(Unit.Value);
        _service = new ShopService(_repository.Object, Mock.Of<ILogger>(), []);
    }

    [Fact]
    public void OrderAndSell_SavesArticle()
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
    public void GetById_ReturnsArticleFromRepository()
    {
        _service.GetById(0);
        _repository.Verify(r => r.GetById(It.IsAny<int>()), Times.Once);
    }
}