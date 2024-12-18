using System;
using System.Collections.Generic;

namespace TheShop
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ISupplier supplier1 = new Supplier1();
            ISupplier supplier2 = new Supplier2();
            ISupplier supplier3 = new Supplier3();
            List<ISupplier> suppliers = [supplier1, supplier2, supplier3];
            var repository = new InMemoryRepository();
            var logger = new ConsoleLogger();
            var shopService = new ShopService(repository, logger, suppliers, TimeProvider.System);

            try
            {
                //order and sell
                shopService.OrderAndSellArticle(1, 20, 10);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            try
            {
                //print article on console
                var article = shopService.GetById(1);
                Console.WriteLine("Found article with ID: " + article.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Article not found: " + ex);
            }

            try
            {
                //print article on console				
                var article = shopService.GetById(12);
                Console.WriteLine("Found article with ID: " + article.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Article not found: " + ex);
            }

            Console.ReadKey();
        }
    }
}