using Core.Articles;

namespace Core.Orders;

public interface IOrder
{
    Result<Article, string> OrderArticle(IEnumerable<ISupplier> suppliers);
}