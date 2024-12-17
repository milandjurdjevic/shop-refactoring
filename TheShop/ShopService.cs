using System;
using System.Collections.Generic;
using System.Linq;

namespace TheShop
{
	public class ShopService
	{
		private DatabaseDriver _databaseDriver;
		private Logger _logger;
		private Supplier1 _supplier1;
		private Supplier2 _supplier2;
		private Supplier3 _supplier3;
		
		public ShopService()
		{
			_databaseDriver = new DatabaseDriver();
			_logger = new Logger();
			_supplier1 = new Supplier1();
			_supplier2 = new Supplier2();
			_supplier3 = new Supplier3();
		}

		public void OrderAndSellArticle(int id, int maxExpectedPrice, int buyerId)
		{
			#region ordering article

			Article article = null;
			Article tempArticle = null;
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
			#endregion

			#region selling article

			if (article == null)
			{
				throw new Exception("Could not order article");
			}

			_logger.Debug("Trying to sell article with id=" + id);

			article.IsSold = true;
			article.SoldOn = DateTime.Now;
			article.SoldTo = buyerId;
			
			try
			{
				_databaseDriver.Save(article);
				_logger.Info("Article with id=" + id + " is sold.");
			}
			catch (ArgumentNullException ex)
			{
				_logger.Error("Could not save article with id=" + id);
				throw new Exception("Could not save article with id");
			}
			catch (Exception)
			{
			}

			#endregion
		}

		public Article GetById(int id)
		{
			return _databaseDriver.GetById(id);
		}
	}

	//in memory implementation
	public class DatabaseDriver
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

	public class Logger
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

	public class Supplier1
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

	public class Supplier2
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

	public class Supplier3
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
		public bool IsSold { get; set; }
		public DateTime? SoldOn { get; set; }
		public int? SoldTo { get; set; }
	}

}
