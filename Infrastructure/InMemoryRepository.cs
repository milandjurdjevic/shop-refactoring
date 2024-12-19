using Core;
using Core.Articles;
using Core.Common;

namespace Infrastructure;

internal class InMemoryRepository : IRepository
{
    private readonly List<SoldArticle> _articles = [];

    public IEnumerable<SoldArticle> Query(int articleId)
    {
        return _articles.Where(x => x.Details.Id == articleId);
    }

    public Result<Unit, string> Save(SoldArticle article)
    {
        _articles.Add(article);
        return Unit.Value;
    }
}