namespace Shop.Core;

public record Article(
    int Id,
    string Name,
    int Price,
    int? BuyerId = null,
    bool IsSold = false,
    DateTime? SoldOn = null
);