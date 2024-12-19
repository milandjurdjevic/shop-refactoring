namespace Shop.Core;

public interface ISale
{
    Result<SoldArticle, string> SellArticle(Article article);
}