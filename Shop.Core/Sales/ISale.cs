using Shop.Core.Articles;

namespace Shop.Core.Sales;

public interface ISale
{
    Result<SoldArticle, string> SellArticle(Article article);
}