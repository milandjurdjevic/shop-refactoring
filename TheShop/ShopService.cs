using System;
using System.Collections.Generic;
using System.Linq;

namespace TheShop
{
    public class ShopService(IRepository repository, ILogger logger, List<ISupplier> suppliers, TimeProvider time)
    {
        public void OrderAndSellArticle(int id, int maxExpectedPrice, int buyerId)
        {
            var article = OrderArticle(id, maxExpectedPrice);
            logger.Debug($"Trying to sell article with id={id}");
            SellArticle(id, buyerId, article);
        }

        private void SellArticle(int id, int buyerId, Article article)
        {
            article.IsSold = true;
            article.SoldOn = time.GetUtcNow().DateTime;
            article.BuyerId = buyerId;

            try
            {
                repository.Save(article);
                logger.Info($"Article with id={id} is sold.");
            }
            catch (ArgumentNullException)
            {
                logger.Error($"Could not save article with id={id}");
                throw new Exception("Could not save article with id");
            }
            catch (Exception e)
            {
                logger.Error("An error occurred while saving the article:\n" + e.Message);
            }
        }

        private Article OrderArticle(int id, int maxExpectedPrice)
        {
            Article? result = null;

            foreach (var supplier in suppliers)
            {
                var articleExist = supplier.ArticleInInventory(id);

                if (!articleExist)
                {
                    break;
                }

                result = supplier.GetArticle(id);

                if (result.Price < maxExpectedPrice)
                {
                    break;
                }
            }

            return result ?? throw new Exception("Could not order article");
        }

        public Article GetById(int id)
        {
            return repository.GetById(id);
        }
    }

    public interface IRepository
    {
        Article GetById(int id);
        void Save(Article article);
    }

    //in memory implementation
    public class InMemoryRepository : IRepository
    {
        private List<Article> _articles = new List<Article>();

        public Article GetById(int id)
        {
            return _articles.Single(x => x.Id == id);
        }

        public void Save(Article article)
        {
            _articles.Add(article);
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
            Console.WriteLine("Info: " + message);
        }

        public void Error(string message)
        {
            Console.WriteLine("Error: " + message);
        }

        public void Debug(string message)
        {
            Console.WriteLine("Debug: " + message);
        }
    }

    public interface ISupplier
    {
        bool ArticleInInventory(int id);
        Article GetArticle(int id);
    }

    public class Supplier1 : ISupplier
    {
        public bool ArticleInInventory(int id)
        {
            return true;
        }

        public Article GetArticle(int id)
        {
            return new Article()
            {
                Id = 1,
                Name = "Article from supplier1",
                Price = 458
            };
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
            return new Article()
            {
                Id = 1,
                Name = "Article from supplier2",
                Price = 459
            };
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
            return new Article()
            {
                Id = 1,
                Name = "Article from supplier3",
                Price = 460
            };
        }
    }

    public class Article
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public int Price { get; set; }
        public int BuyerId { get; set; }
        public bool IsSold { get; set; }
        public DateTime? SoldOn { get; set; }
    }
}