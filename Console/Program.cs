using System;
using System.Collections.Generic;
using System.Linq;

using Core;
using Core.Common;
using Core.Orders;
using Core.Sales;

using Infrastructure;

ISupplier supplier1 = new ExternalSupplier1();
ISupplier supplier2 = new ExternalSupplier2();
ISupplier supplier3 = new ExternalSupplier3();
List<ISupplier> suppliers = [supplier1, supplier2, supplier3];
InMemoryRepository repository = new();
ConsoleLogger logger = new();
Shop shop = new(repository, logger, suppliers);

TryOrderAndSell(new DefaultOrder(1, 20), new DefaultSale(10, TimeProvider.System));
TryFind(1);
TryFind(22);
TryOrderAndSell(new BestPriceOrder(1, 460), new DefaultSale(10, TimeProvider.System));
TryOrderAndSell(new BestPriceOrder(1, 460), new BudgetSale(10, 400, TimeProvider.System));
TryFind(1);

Console.ReadKey();

return;

void TryFind(int id)
{
    int count = shop.GetSoldArticles(id).Count();

    if (count > 0)
    {
        logger.Debug($"Found {count} sold articles by ID({id})");
    }
    else
    {
        logger.Error($"Not found any sold articles by ID({id})");
    }
}

void TryOrderAndSell(IOrder order, ISale sale)
{
    shop
        .OrderAndSellArticle(order, sale)
        .Switch(_ => logger.Info("Article is ordered and sold"), err => logger.Error(err));
}