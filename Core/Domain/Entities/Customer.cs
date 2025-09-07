namespace Domain.Entities;

public class Customer
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string Name { get; private set; }
    public string HashedPassword { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public string Role { get; private set; }

    public ICollection<Order> Orders { get; private set; } = new List<Order>();

    private Customer() 
    {
        Email = string.Empty;
        Name = string.Empty;
        HashedPassword = string.Empty;
        Role = string.Empty;
    }

    public Customer(Guid id, string email, string name, string hashedPassword, string role)
    {
        Id = id;
        Email = email;
        Name = name;
        HashedPassword = hashedPassword;
        CreatedAt = DateTime.UtcNow;
        Role = role;
    }
}