namespace Shop.Core;

public class ShopService(IRepository repository, ILogger logger, List<ISupplier> suppliers, TimeProvider time)
{
    public Article? GetById(int id)
    {
        return repository.GetById(id);
    }

    public Result<Unit, string> OrderAndSellArticle(IOrder order, int buyerId)
    {
        return order.OrderArticle(suppliers)
            .Tap(val => logger.Debug($"Ordered: {val}"))
            .Map(val => SellArticle(val, time.GetUtcNow(), buyerId))
            .Tap(val => logger.Debug($"Sold: {val}"))
            .Bind(repository.Save);
    }

    private static Article SellArticle(Article article, DateTimeOffset timestamp, int buyerId)
    {
        return article with { IsSold = true, SoldOn = timestamp.DateTime, BuyerId = buyerId };
    }
}