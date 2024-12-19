using Shop.Core.Articles;

namespace Shop.Core.Orders;

public interface IOrder
{
    Result<Article, string> OrderArticle(IEnumerable<ISupplier> suppliers);
}