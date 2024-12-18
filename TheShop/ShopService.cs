using System;
using System.Collections.Generic;
using System.Linq;

namespace TheShop
{
	public class ShopService
	{
		private IRepository _repository;
		private ILogger _logger;
		private ISupplier _supplier1;
		private ISupplier _supplier2;
		private ISupplier _supplier3;
		
		public ShopService(IRepository repository, ILogger logger, ISupplier supplier1, ISupplier supplier2, ISupplier supplier3)
		{
			_repository = repository;
			_logger = logger;
			_supplier1 = supplier1;
			_supplier2 = supplier2;
			_supplier3 = supplier3;
		}

		public void OrderAndSellArticle(int id, int maxExpectedPrice, int buyerId)
		{
			var article = OrderArticle(id, maxExpectedPrice);
			_logger.Debug($"Trying to sell article with id={id}");
			SellArticle(id, buyerId, article);
		}

		private void SellArticle(int id, int buyerId, Article article)
		{
			article.IsSold = true;
			article.SoldOn = DateTime.Now;
			article.BuyerId = buyerId;
			
			try
			{
				_repository.Save(article);
				_logger.Info($"Article with id={id} is sold.");
			}
			catch (ArgumentNullException)
			{
				_logger.Error($"Could not save article with id={id}");
				throw new Exception("Could not save article with id");
			}
			catch (Exception e)
			{
				_logger.Error("An error occurred while saving the article:\n" + e.Message);
			}
		}

		private Article OrderArticle(int id, int maxExpectedPrice)
		{
			Article? article = null;
			Article? tempArticle = null;
			var articleExists = _supplier1.ArticleInInventory(id);
			if (articleExists)
			{
				tempArticle = _supplier1.GetArticle(id);
				if (maxExpectedPrice < tempArticle.Price)
				{
					articleExists = _supplier2.ArticleInInventory(id);
					if (articleExists)
					{
						tempArticle = _supplier2.GetArticle(id);
						if (maxExpectedPrice < tempArticle.Price)
						{
							articleExists = _supplier3.ArticleInInventory(id);
							if (articleExists)
							{
								tempArticle = _supplier3.GetArticle(id);
								if (maxExpectedPrice < tempArticle.Price)
								{
									article = tempArticle;
								}
							}
						}
					}
				}
			}
			
			article = tempArticle;

			if (article == null)
			{
				throw new Exception("Could not order article");
			}

			return article;
		}

		public Article GetById(int id)
		{
			return _repository.GetById(id);
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
