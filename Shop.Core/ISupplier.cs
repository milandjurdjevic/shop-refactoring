using Shop.Core.Articles;

namespace Shop.Core;

public interface ISupplier
{
    bool ArticleInInventory(int id);
    Article? GetArticle(int id);
}