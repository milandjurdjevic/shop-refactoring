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
    private readonly Mock<ISupplier> _supplier1 = new();
    private readonly Mock<ISupplier> _supplier2 = new();
    private readonly Mock<ISupplier> _supplier3 = new();
    private readonly Mock<TimeProvider> _time = new();

    public ShopServiceTest()
    {
        _repository.Setup(r => r.Save(It.IsAny<Article>())).Returns(Unit.Value);
        List<ISupplier> suppliers = [_supplier1.Object, _supplier2.Object, _supplier3.Object];
        ILogger logger = Mock.Of<ILogger>();
        _service = new ShopService(_repository.Object, logger, suppliers, _time.Object);
    }

    [Fact]
    public void OrderAndSellArticle_SavesArticleSoldToBuyer()
    {
        _supplier1.Setup(s => s.ArticleInInventory(It.IsAny<int>())).Returns(true);
        _supplier1.Setup(s => s.GetArticle(It.IsAny<int>())).Returns(new Article(0, string.Empty, 0));
        DateTimeOffset expectedTime = DateTimeOffset.MinValue;
        _time.Setup(r => r.GetUtcNow()).Returns(expectedTime);
        _service.OrderAndSellArticle(new DefaultOrder(1, 0), 1);
        _repository.Verify(
            r => r.Save(It.Is<Article>(a => a.BuyerId == 1 && a.IsSold && a.SoldOn == expectedTime.DateTime)),
            Times.Once
        );
    }
    

    [Fact]
    public void GetById_ReturnsArticleFromRepository()
    {
        _service.GetById(0);
        _repository.Verify(r => r.GetById(It.IsAny<int>()), Times.Once);
    }
}