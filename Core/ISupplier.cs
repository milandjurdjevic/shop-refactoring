using Core.Articles;

namespace Core;

public interface ISupplier
{
    bool ArticleInInventory(int id);
    Article? GetArticle(int id);
}