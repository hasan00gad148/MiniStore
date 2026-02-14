namespace MiniStore.Models
{
    public class CartItem
    {
        public required Product product{ get; set; }
        public int Quantity { get; set; }
        public decimal LineTotal => product.Price * Quantity;
        
    }
}