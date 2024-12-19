using Core.Articles;

namespace Core;

public interface IRepository
{
    IEnumerable<SoldArticle> Query(int articleId);
    Result<Unit, string> Save(SoldArticle article);
}