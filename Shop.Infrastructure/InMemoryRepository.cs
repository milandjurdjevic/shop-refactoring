using Shop.Core;
using Shop.Core.Articles;

namespace Shop.Infrastructure;

public class InMemoryRepository : IRepository
{
    private readonly List<SoldArticle> _articles = [];

    public SoldArticle? GetById(int id)
    {
        return _articles.SingleOrDefault(x => x.Details.Id == id);
    }

    public Result<Unit, string> Save(SoldArticle article)
    {
        _articles.Add(article);
        return Unit.Value;
    }
}