using Core.Articles;
using Core.Orders;
using Core.Sales;

namespace Core;

public class Shop(IRepository repository, ILogger logger, List<ISupplier> suppliers)
{
    public IEnumerable<SoldArticle> GetSoldArticles(int id)
    {
        return repository.Query(id);
    }

    public Result<Unit, string> OrderAndSellArticle(IOrder order, ISale sale)
    {
        return order.OrderArticle(suppliers)
            .Tap(val => logger.Debug($"Ordered: {val}"))
            .Bind(sale.SellArticle)
            .Tap(val => logger.Debug($"Sold: {val}"))
            .Bind(repository.Save);
    }
}