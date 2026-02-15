using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniStore.Models;
using MiniStore.Repos;



namespace MiniStore.Services
{
    public class OrderService
    {
            private readonly JsonRepo<Order> _orderRepo;
    private readonly ProductService _productService;
    private readonly CartService _cartService;

    public OrderService(string orderPath, ProductService productService, CartService cartService)
    {
        _orderRepo = new JsonRepo<Order>(orderPath);
        _productService = productService;
        _cartService = cartService;
    }

    public void Checkout(User user)
    {
        var cart = _cartService.GetCart(user.Id);
        if (!cart.items.Any()) { Console.WriteLine("❌ Cart is empty."); return; }

        var products = _productService.GetAll();

        foreach (var item in cart.items)
        {
            var product = products.FirstOrDefault(p => p.Id == item.ProductId);
            if (product == null) { Console.WriteLine($"❌ Product {item.ProductName} missing."); return; }
            if (product.Stock < item.Quantity) { Console.WriteLine($"❌ Not enough stock for {item.ProductName}."); return; }
        }

        foreach (var item in cart.items)
        {
            var product = products.First(p => p.Id == item.ProductId);
            product.Stock -= item.Quantity;
        }

        _productService.Save(products);

        var order = new Order
        {
            UserId = user.Id,
            Items = cart.items.Select(i => new CartItem { ProductId = i.ProductId, ProductName = i.ProductName, ProductPrice = i.ProductPrice, Quantity = i.Quantity }).ToList(),
            Total = cart.Total,
            CreatedAt = DateTime.UtcNow
        };

        var orders = _orderRepo.ReadAll();
        orders.Add(order);
        _orderRepo.WriteAll(orders);

        _cartService.Clear(user.Id);

        Console.WriteLine($"✅ Order placed. Total: {order.Total:C}");
    }
    }
}