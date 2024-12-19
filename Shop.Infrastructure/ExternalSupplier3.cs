using Shop.Core;
using Shop.Core.Articles;

namespace Shop.Infrastructure;

public class ExternalSupplier3 : ISupplier
{
    public bool ArticleInInventory(int id)
    {
        return true;
    }

    public Article GetArticle(int id)
    {
        return new Article(1, "Article from supplier3", 460);
    }
}