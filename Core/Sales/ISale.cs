using Core.Articles;

namespace Core.Sales;

public interface ISale
{
    Result<SoldArticle, string> SellArticle(Article article);
}