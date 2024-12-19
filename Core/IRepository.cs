using Core.Articles;
using Core.Common;

namespace Core;

public interface IRepository
{
    IEnumerable<SoldArticle> Query(int articleId);
    Result<Unit, string> Save(SoldArticle article);
}