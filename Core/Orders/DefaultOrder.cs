using Core.Articles;

namespace Core.Orders;

// Original ordering implementation.
public class DefaultOrder(int articleId, int maxPrice) : IOrder
{
    public Result<Article, string> OrderArticle(IEnumerable<ISupplier> suppliers)
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