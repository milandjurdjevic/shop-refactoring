namespace Shop.Core;

public class ShopService(IRepository repository, ILogger logger, List<ISupplier> suppliers, TimeProvider time)
{
    public Article? GetById(int id)
    {
        return repository.GetById(id);
    }

    public Result<Unit, string> OrderAndSellArticle(int id, int maxExpectedPrice, int buyerId)
    {
        return OrderArticle(suppliers, id, maxExpectedPrice)
            .Tap(val => logger.Debug($"Ordered: {val}"))
            .Map(val => SellArticle(val, time.GetUtcNow(), buyerId))
            .Tap(val => logger.Debug($"Sold: {val}"))
            .Bind(repository.Save);
    }

    private static Article SellArticle(Article article, DateTimeOffset timestamp, int buyerId)
    {
        return article with { IsSold = true, SoldOn = timestamp.DateTime, BuyerId = buyerId };
    }

    private static Result<Article, string> OrderArticle(List<ISupplier> suppliers, int articleId, int maxPrice)
    {
        Article? result = null;
        // Order of suppliers is important. Here are some rules captured during refactoring:
        // - If the first supplier does not have the article, the second supplier won't be checked.
        // - If there are no suppliers with a price matched article, the last one will be returned.
        // - if a supplier does not have the article in inventory, the article from the last supplier will be result.
        foreach (ISupplier supplier in suppliers)
        {
            bool articleExist = supplier.ArticleInInventory(articleId);

            if (!articleExist)
            {
                break;
            }

            result = supplier.GetArticle(articleId);

            if (result?.Price < maxPrice)
            {
                break;
            }
        }

        return result != null ? result : "Could not order article";
    }
}