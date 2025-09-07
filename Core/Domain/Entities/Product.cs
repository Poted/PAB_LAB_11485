using System.Security.Cryptography;

namespace Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Sku { get; private set; }
    public decimal Price { get; private set; }

    private Product() {
        Name = string.Empty;
        Sku = string.Empty;
    }

    public Product(Guid id, string name, string sku, decimal price)
    {
        Id = id;
        Name = name;
        Sku = sku;
        Price = price;
    }

    public void UpdateDetails(string newName, string newSku, decimal newPrice)
    {
        Name = newName;
        Sku = newSku;
        Price = newPrice;
    }

}
