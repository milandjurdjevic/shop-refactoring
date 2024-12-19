using Shop.Core.Articles;

namespace Shop.Core;

public interface IRepository
{
    SoldArticle? GetById(int id);
    Result<Unit, string> Save(SoldArticle article);
}