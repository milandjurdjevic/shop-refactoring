using System;
using System.Collections.Generic;
using System.Linq;

namespace TheShop
{
    public struct Unit
    {
        public static readonly Unit Value = new();
    }

    public class ShopService(IRepository repository, ILogger logger, List<ISupplier> suppliers, TimeProvider time)
    {
        public Article? GetById(int id) => repository.GetById(id);

        public Result<Unit, string> OrderAndSellArticle(int id, int maxExpectedPrice, int buyerId) =>
            OrderArticle(suppliers, id, maxExpectedPrice)
                .Tap(val => logger.Debug($"Ordered: {val}"))
                .Map(val => SellArticle(val, time.GetUtcNow(), buyerId))
                .Tap(val => logger.Debug($"Sold: {val}"))
                .Bind(repository.Save);

        private static Article SellArticle(Article article, DateTimeOffset timestamp, int buyerId) =>
            article with { IsSold = true, SoldOn = timestamp.DateTime, BuyerId = buyerId };

        private static Result<Article, string> OrderArticle(List<ISupplier> suppliers, int articleId, int maxPrice)
        {
            Article? result = null;
            // Order of suppliers is important. Here are some rules captured during refactoring:
            // - If the first supplier does not have the article, the second supplier won't be checked.
            // - If there are no suppliers with a price matched article, the last one will be returned.
            // - if a supplier does not have the article in inventory, the article from the last supplier will be result.
            foreach (var supplier in suppliers)
            {
                var articleExist = supplier.ArticleInInventory(articleId);

                if (!articleExist)
                {
                    break;
                }

                result = supplier.GetArticle(articleId);

                if (result?.Price < maxPrice)
                {
                    break;
                }
            }

            return result != null ? result : "Could not order article";
        }
    }

    public interface IRepository
    {
        Article? GetById(int id);
        Result<Unit, string> Save(Article article);
    }

    //in memory implementation
    public class InMemoryRepository : IRepository
    {
        private List<Article> _articles = [];

        public Article? GetById(int id)
        {
            return _articles.SingleOrDefault(x => x.Id == id);
        }

        public Result<Unit, string> Save(Article article)
        {
            _articles.Add(article);
            return Unit.Value;
        }
    }

    public interface ILogger
    {
        void Info(string message);
        void Error(string message);
        void Debug(string message);
    }

    public class ConsoleLogger : ILogger
    {
        public void Info(string message)
        {
            WriteLine("[INFO] " + message, ConsoleColor.DarkBlue);
        }

        public void Error(string message)
        {
            WriteLine("[ERROR] " + message, ConsoleColor.DarkRed);
        }

        public void Debug(string message)
        {
            WriteLine("[DEBUG] " + message, ConsoleColor.DarkMagenta);
        }

        private static void WriteLine(string message, ConsoleColor color)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = originalColor;
        }
    }

    public interface ISupplier
    {
        bool ArticleInInventory(int id);
        Article? GetArticle(int id);
    }

    public class Supplier1 : ISupplier
    {
        public bool ArticleInInventory(int id)
        {
            return true;
        }

        public Article GetArticle(int id)
        {
            return new Article(1, "Article from supplier1", 458);
        }
    }

    public class Supplier2 : ISupplier
    {
        public bool ArticleInInventory(int id)
        {
            return true;
        }

        public Article GetArticle(int id)
        {
            return new Article(1, "Article from supplier2", 459);
        }
    }

    public class Supplier3 : ISupplier
    {
        public bool ArticleInInventory(int id)
        {
            return true;
        }

        public Article GetArticle(int id)
        {
            return new Article(1, "Article from supplier3", 460);
        }
    }

    public record Article(
        int Id,
        string Name,
        int Price,
        int? BuyerId = null,
        bool IsSold = false,
        DateTime? SoldOn = null
    );
}