using System;
using System.Collections.Generic;

using Shop.Core;
using Shop.Core.Orders;
using Shop.Core.Sales;
using Shop.Infrastructure;

ISupplier supplier1 = new ExternalSupplier1();
ISupplier supplier2 = new ExternalSupplier2();
ISupplier supplier3 = new ExternalSupplier3();
List<ISupplier> suppliers = [supplier1, supplier2, supplier3];
InMemoryRepository repository = new();
ConsoleLogger logger = new();
ShopService service = new(repository, logger, suppliers);

TryOrderAndSell(new DefaultOrder(1, 20), new DefaultSale(10, TimeProvider.System));
TryFind(1);
TryFind(22);
TryOrderAndSell(new BestPriceOrder(1, 460), new DefaultSale(10, TimeProvider.System));
TryOrderAndSell(new BestPriceOrder(1, 460), new BudgetSale(10, 400, TimeProvider.System));

Console.ReadKey();

return;

void TryFind(int id)
{
    if (service.GetById(id) is { } article)
    {
        logger.Debug($"Found: {article}");
    }
    else
    {
        logger.Error($"Not found: ID({id})");
    }
}

void TryOrderAndSell(IOrder order, ISale sale)
{
    service
        .OrderAndSellArticle(order, sale)
        .Switch(_ => logger.Info("Article is ordered and sold"), err => logger.Error(err));
}