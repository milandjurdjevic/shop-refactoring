using JetBrains.Annotations;
using Moq;

namespace TheShop.Tests;

[TestSubject(typeof(ShopService))]
public class ShopServiceTest
{
    private readonly Mock<IRepository> _repository = new();
    private readonly Mock<ISupplier> _supplier1 = new();
    private readonly Mock<ISupplier> _supplier2 = new();
    private readonly Mock<ISupplier> _supplier3 = new();
    private readonly Mock<TimeProvider> _time = new();

    private readonly ShopService _service;

    public ShopServiceTest()
    {
        var suppliers = new List<ISupplier> { _supplier1.Object, _supplier2.Object, _supplier3.Object };
        var logger = Mock.Of<ILogger>();
        _service = new ShopService(_repository.Object, logger, suppliers, _time.Object);
    }

    [Fact]
    public void OrderAndSellArticle_SavesFirstArticleBelowThreshold()
    {
        _supplier1.Setup(s => s.ArticleInInventory(It.IsAny<int>())).Returns(true);
        _supplier1.Setup(s => s.GetArticle(It.IsAny<int>())).Returns(new Article { Price = 100 });
        _supplier2.Setup(s => s.ArticleInInventory(It.IsAny<int>())).Returns(true);
        _supplier2.Setup(s => s.GetArticle(It.IsAny<int>())).Returns(new Article { Price = 90 });
        _supplier3.Setup(s => s.ArticleInInventory(It.IsAny<int>())).Returns(true);
        _supplier3.Setup(s => s.GetArticle(It.IsAny<int>())).Returns(new Article { Price = 80 });
        _service.OrderAndSellArticle(0, 95, 0);
        _repository.Verify(r => r.Save(It.Is<Article>(a => a.Price == 90)), Times.Once);
    }

    [Fact]
    public void OrderAndSellArticle_SavesArticleSoldToBuyer()
    {
        _supplier1.Setup(s => s.ArticleInInventory(It.IsAny<int>())).Returns(true);
        _supplier1.Setup(s => s.GetArticle(It.IsAny<int>())).Returns(new Article());
        var expectedTime = DateTimeOffset.MinValue;
        _time.Setup(r => r.GetUtcNow()).Returns(expectedTime);
        _service.OrderAndSellArticle(0, 0, 1);
        _repository.Verify(
            r => r.Save(It.Is<Article>(a => a.BuyerId == 1 && a.IsSold && a.SoldOn == expectedTime.DateTime)),
            Times.Once
        );
    }

    [Fact]
    public void OrderAndSellArticle_NoArticleInInventory_SavesTheLastArticleInInventory()
    {
        _supplier1.Setup(s => s.ArticleInInventory(It.IsAny<int>())).Returns(true);
        _supplier1.Setup(s => s.GetArticle(It.IsAny<int>())).Returns(new Article { Price = 90 });
        _supplier2.Setup(s => s.ArticleInInventory(It.IsAny<int>())).Returns(false);
        _supplier3.Setup(s => s.ArticleInInventory(It.IsAny<int>())).Returns(true);
        _service.OrderAndSellArticle(0, 85, 0);
        _repository.Verify(r => r.Save(It.Is<Article>(a => a.Price == 90)), Times.Once);
    }

    [Fact]
    public void OrderAndSellArticle_NoArticleBelowThreshold_SavesLastArticle()
    {
        _supplier1.Setup(s => s.ArticleInInventory(It.IsAny<int>())).Returns(true);
        _supplier1.Setup(s => s.GetArticle(It.IsAny<int>())).Returns(new Article { Id = 1, Price = 100 });
        _supplier2.Setup(s => s.ArticleInInventory(It.IsAny<int>())).Returns(true);
        _supplier2.Setup(s => s.GetArticle(It.IsAny<int>())).Returns(new Article { Id = 1, Price = 80 });
        _supplier3.Setup(s => s.ArticleInInventory(It.IsAny<int>())).Returns(true);
        _supplier3.Setup(s => s.GetArticle(It.IsAny<int>())).Returns(new Article { Id = 1, Price = 90 });
        _service.OrderAndSellArticle(0, 70, 0);
        _repository.Verify(r => r.Save(It.Is<Article>(a => a.Price == 90)), Times.Once);
    }

    [Fact]
    public void OrderAndSellArticle_ArticleNotFound_ThrowsException() =>
        Assert.Throws<Exception>(() => _service.OrderAndSellArticle(0, 0, 0));

    [Fact]
    public void GetById_ReturnsArticleFromRepository()
    {
        _service.GetById(0);
        _repository.Verify(r => r.GetById(It.IsAny<int>()), Times.Once);
    }
}