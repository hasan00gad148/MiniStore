
namespace MiniStore.Models
{
    public class Cart
    {
        public Guid UserId { get; set; }
        public List<CartItem> items { get; set; } = new List<CartItem>();
        public decimal Total => items.Sum(x => x.LineTotal);
    }
}