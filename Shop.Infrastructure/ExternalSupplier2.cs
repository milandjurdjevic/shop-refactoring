using Shop.Core;

namespace Shop.Infrastructure;

public class ExternalSupplier2 : ISupplier
{
    public bool ArticleInInventory(int id)
    {
        return true;
    }

    public Article GetArticle(int id)
    {
        return new Article(1, "Article from supplier2", 459);
    }
}