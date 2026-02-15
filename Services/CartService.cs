using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniStore.Models;
using MiniStore.Repos;




namespace MiniStore.Services
{
    public class CartService
    {
        private readonly JsonRepo<Cart> _repo;
        private readonly ProductService _productService;

        public CartService(string path, ProductService productService)
        {
            _repo = new JsonRepo<Cart>(path);
            _productService = productService;
        }

        public Cart GetCart(Guid userId)
        {
            var carts = _repo.ReadAll();
            var cart = carts.FirstOrDefault(c => c.UserId == userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                carts.Add(cart);
                _repo.WriteAll(carts);
            }
            return cart;
        }

        public void AddToCart(Guid userId, Guid productId, int quantity)
        {
            if (quantity <= 0) { Console.WriteLine("❌ Quantity must be > 0."); return; }
            var product = _productService.GetById(productId);
            if (product == null) { Console.WriteLine("❌ Product not found."); return; }
            if (product.Stock < quantity) { Console.WriteLine("❌ Not enough stock."); return; }

            var carts = _repo.ReadAll();
            var cart = carts.FirstOrDefault(c => c.UserId == userId);
            if (cart == null) { cart = new Cart { UserId = userId }; carts.Add(cart); }

            var existing = cart.items.FirstOrDefault(i => i.ProductId == productId);
            if (existing != null) existing.Quantity += quantity;
            else cart.items.Add(new CartItem { ProductId = product.Id, ProductName = product.Name, ProductPrice = product.Price, Quantity = quantity });

            _repo.WriteAll(carts);
            Console.WriteLine("✅ Added to cart.");
        }

        public void Clear(Guid userId)
        {
            var carts = _repo.ReadAll();
            var cart = carts.FirstOrDefault(c => c.UserId == userId);
            if (cart != null) { cart.items.Clear(); _repo.WriteAll(carts); }
        }
    }
}