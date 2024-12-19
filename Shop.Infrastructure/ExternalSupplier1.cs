using Shop.Core;

namespace Shop.Infrastructure;

public class ExternalSupplier1 : ISupplier
{
    public bool ArticleInInventory(int id)
    {
        return true;
    }

    public Article GetArticle(int id)
    {
        return new Article(1, "Article from supplier1", 458);
    }
}