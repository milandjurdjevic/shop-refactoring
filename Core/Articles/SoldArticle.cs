namespace Core.Articles;

public record SoldArticle(Article Details, DateTimeOffset Timestamp, int BuyerId);