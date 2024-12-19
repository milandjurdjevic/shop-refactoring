namespace Shop.Core;

public interface IOrder
{
    Result<Article, string> OrderArticle(IEnumerable<ISupplier> suppliers);
}