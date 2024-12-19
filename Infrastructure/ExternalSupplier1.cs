using Core;
using Core.Articles;

namespace Infrastructure;

internal class ExternalSupplier1 : ISupplier
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