using Shop.Core.Articles;

namespace Shop.Core.Orders;

public class BestPriceOrder(int id, int threshold) : IOrder
{
    public Result<Article, string> OrderArticle(IEnumerable<ISupplier> suppliers)
    {
        Article? article = suppliers
            .Where(s => s.ArticleInInventory(id))
            .Select(s => s.GetArticle(id))
            .OfType<Article>()
            .Where(a => a.Price <= threshold)
            .OrderBy(a => a.Price)
            .FirstOrDefault();

        return article != null ? article : "Could not order article";
    }
}