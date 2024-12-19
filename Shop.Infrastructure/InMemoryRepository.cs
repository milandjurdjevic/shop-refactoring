using Shop.Core;

namespace Shop.Infrastructure;

public class InMemoryRepository : IRepository
{
    private List<Article> _articles = [];

    public Article? GetById(int id)
    {
        return _articles.SingleOrDefault(x => x.Id == id);
    }

    public Result<Unit, string> Save(Article article)
    {
        _articles.Add(article);
        return Unit.Value;
    }
}