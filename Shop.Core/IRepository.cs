using Shop.Core.Articles;

namespace Shop.Core;

public interface IRepository
{
    IEnumerable<SoldArticle> Query(int articleId);
    Result<Unit, string> Save(SoldArticle article);
}