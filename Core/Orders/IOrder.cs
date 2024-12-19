using Core.Articles;
using Core.Common;

namespace Core.Orders;

public interface IOrder
{
    Result<Article, string> OrderArticle(IEnumerable<ISupplier> suppliers);
}