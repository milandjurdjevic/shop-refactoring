using System;
using System.Collections.Generic;

using Shop.Core;
using Shop.Infrastructure;

ISupplier supplier1 = new ExternalSupplier1();
ISupplier supplier2 = new ExternalSupplier2();
ISupplier supplier3 = new ExternalSupplier3();
List<ISupplier> suppliers = [supplier1, supplier2, supplier3];
InMemoryRepository repository = new();
ConsoleLogger logger = new();
ShopService service = new(repository, logger, suppliers);

service
    .OrderAndSellArticle(new DefaultOrder(1, 20), new DefaultSale(10, TimeProvider.System))
    .Switch(_ => logger.Info("Article is ordered and sold"), err => logger.Error(err));

TryFindAndPrint(1);
TryFindAndPrint(22);

Console.ReadKey();

return;

void TryFindAndPrint(int id)
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