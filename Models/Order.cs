namespace MiniStore.Models;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required User user{ get; set; }   
    public List<CartItem> Items { get; set; } = new List<CartItem>();
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}