using Core;

namespace Infrastructure;

public static class ShopFactory
{
    public static readonly ILogger Logger = new ConsoleLogger();

    public static Shop Create()
    {
        ISupplier supplier1 = new ExternalSupplier1();
        ISupplier supplier2 = new ExternalSupplier2();
        ISupplier supplier3 = new ExternalSupplier3();
        List<ISupplier> suppliers = [supplier1, supplier2, supplier3];
        InMemoryRepository repository = new();
        return new Shop(repository, Logger, suppliers);
    }
}