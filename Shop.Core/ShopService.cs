using Shop.Core.Articles;
using Shop.Core.Orders;
using Shop.Core.Sales;

namespace Shop.Core;

public class ShopService(IRepository repository, ILogger logger, List<ISupplier> suppliers)
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