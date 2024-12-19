using Core.Articles;

namespace Core.Sales;

public class BudgetSale(int buyerId, int budget, TimeProvider time) : ISale
{
    private  int _budget = budget;

    public Result<SoldArticle, string> SellArticle(Article article)
    {
        if (_budget < article.Price)
        {
            return "Not enough money";
        }
        
        _budget -= article.Price;
        return new SoldArticle(article, time.GetUtcNow(), buyerId);
    }
}