using System;
using System.Collections.Generic;
using TheShop;

ISupplier supplier1 = new Supplier1();
ISupplier supplier2 = new Supplier2();
ISupplier supplier3 = new Supplier3();
List<ISupplier> suppliers = [supplier1, supplier2, supplier3];
var repository = new InMemoryRepository();
var logger = new ConsoleLogger();
var service = new ShopService(repository, logger, suppliers, TimeProvider.System);

service
    .OrderAndSellArticle(1, 20, 10)
    .Switch(_ => logger.Info("Article is ordered and sold"), err => logger.Error(err));

if (service.GetById(1) is { } article1)
{
    logger.Info($"Found by id(1): {article1}");
}
else
{
    logger.Error("Not found by id(1)");
}

if (service.GetById(22) is { } article22)
{
    logger.Info($"Found by id(22): {article22}");
}
else
{
    logger.Error("Not found by id(22)");
}

Console.ReadKey();