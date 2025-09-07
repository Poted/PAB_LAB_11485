namespace Domain.Entities;

public class OrderItem
{
    public Guid Id { get; private set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }

    public Guid OrderId { get; private set; }
    public Order? Order { get; private set; }

    public Guid ProductId { get; private set; }
    public Product? Product { get; private set; }

    private OrderItem() { }

    public OrderItem(Guid id, Guid orderId, Guid productId, int quantity, decimal price)
    {
        Id = id;
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
        Price = price;
    }
}