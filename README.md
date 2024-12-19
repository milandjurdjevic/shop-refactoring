# Shop Refactoring

## Project Description

This project is a refactoring of a shop system. The system allows for ordering and selling articles, querying sold
articles, and logging activities. The project is structured into several components, each responsible for different
aspects of the system.

## Project Structure

- **Core**: Contains the core business logic and domain models.
- **Infrastructure**: Contains the implementation of repositories, factories, and other infrastructure-related classes.
- **Console**: Contains the entry point of the application.
- **Tests**: Contains unit tests for the core business logic.

## Domain Diagram

```mermaid
classDiagram
    class Shop {
        - IRepository repository
        - ILogger logger
        - IEnumerable~ISupplier~ suppliers
        + Result~Unit, string~ OrderAndSellArticle(IOrder order, ISale sale)
        + IEnumerable~SoldArticle~ GetSoldArticles(int articleId)
    }

    class IRepository {
        <<interface>>
        + IEnumerable~SoldArticle~ Query(int articleId) 
        + Result~Unit, string~ Save(SoldArticle article)
    }

    class ILogger {
        <<interface>>
        + void Info(string message)
        + void Warning(string message)
        + void Error(string message)
    }

    class ISupplier {
        <<interface>>
        + bool ArticleInInventory(int id)
        + Article? GetArticle(int id)
    }
    
    class Article {
        +int Id
        +string Name
        +int Price
    }

    class SoldArticle {
        +Article Details
        +DateTimeOffset Timestamp
        +int BuyerId
    }

    class ISale {
        <<interface>>
        +Result~SoldArticle, string~ SellArticle(Article article)
    }

    class DefaultSale {
        - int buyerId
        - TimeProvider time
        +Result~SoldArticle, string~ SellArticle(Article article)
    }

    class BudgetSale {
        - int buyerId
        - int budget
        - TimeProvider time
        +Result~SoldArticle, string~ SellArticle(Article article)
    }

    class IOrder {
        <<interface>>
        +Result~Article, string~ OrderArticle(IEnumerable~ISupplier~ suppliers);
    }

    class DefaultOrder {
        - int articleId
        - int maxPrice
        +Result~Article, string~ OrderArticle(IEnumerable~ISupplier~ suppliers);
    }

    class BestPriceOrder {
        - int id
        - int threshold
        +Result~Article, string~ OrderArticle(IEnumerable~ISupplier~ suppliers);
    }
    
    Shop --> IRepository
    Shop --> ILogger
    Shop --> ISupplier
    Shop --> ISale
    Shop --> IOrder
    Shop --> SoldArticle
    Shop --> Article
    
    ISale --> Article
    ISale --> SoldArticle
    
    IOrder --> Article

    DefaultSale ..|> ISale
    BudgetSale ..|> ISale

    DefaultOrder ..|> IOrder
    BestPriceOrder ..|> IOrder

    SoldArticle --> Article