namespace Shop.Core;

public record Article(int Id, string Name, int Price);

public record SoldArticle(Article Details, DateTimeOffset Timestamp, int BuyerId);