using Core.Articles;
using Core.Common;

namespace Core.Sales;

public interface ISale
{
    Result<SoldArticle, string> SellArticle(Article article);
}