using System;
using System.Linq;

using Core;
using Core.Common;
using Core.Orders;
using Core.Sales;

using Infrastructure;

Shop shop = ShopFactory.Create();

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
        ShopFactory.Logger.Info($"Found {count} sold articles by ID({id})");
    }
    else
    {
        ShopFactory.Logger.Warning($"Not found any sold articles by ID({id})");
    }
}

void TryOrderAndSell(IOrder order, ISale sale)
{
    shop
        .OrderAndSellArticle(order, sale)
        .Switch(_ => ShopFactory.Logger.Info("Article is ordered and sold"), err => ShopFactory.Logger.Error(err));
}