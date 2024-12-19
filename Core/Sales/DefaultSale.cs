using Core.Articles;

namespace Core.Sales;

public class DefaultSale(int buyerId, TimeProvider time) : ISale
{
    public Result<SoldArticle, string> SellArticle(Article article)
    {
        return new SoldArticle(article, time.GetUtcNow(), buyerId);
    }
}