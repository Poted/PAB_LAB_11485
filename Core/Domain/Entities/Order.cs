namespace Domain.Entities;

public class Order
{
    public Guid Id { get; private set; }
    public DateTime OrderDate { get; private set; }
    public decimal TotalPrice => OrderItems.Sum(item => item.Quantity * item.Price);

    public Guid CustomerId { get; private set; }
    public Customer? Customer { get; private set; }

    public ICollection<OrderItem> OrderItems { get; private set; } = new List<OrderItem>();

    private Order() { }

    public Order(Guid id, Guid customerId)
    {
        Id = id;
        CustomerId = customerId;
        OrderDate = DateTime.UtcNow;
    }
}