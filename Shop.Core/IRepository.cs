namespace Shop.Core;

public interface IRepository
{
    Article? GetById(int id);
    Result<Unit, string> Save(Article article);
}