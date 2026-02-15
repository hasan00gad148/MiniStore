using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniStore.Models;
using MiniStore.Repos;



namespace MiniStore.Services
{
    public class ProductService
    {
            private readonly JsonRepo<Product> _repo;

    public ProductService(string path) => _repo = new JsonRepo<Product>(path);

    public List<Product> GetAll() => _repo.ReadAll().OrderBy(p => p.Name).ToList();
    public Product? GetById(Guid id) => _repo.ReadAll().FirstOrDefault(p => p.Id == id);

    public void Add(string name, decimal price, int stock)
    {
        if (price <= 0 || stock < 0)
        {
            Console.WriteLine("❌ Invalid price or stock.");
            return;
        }

        var products = _repo.ReadAll();
        products.Add(new Product { Name = name, Price = price, Stock = stock });
        _repo.WriteAll(products);
        Console.WriteLine("✅ Product added.");
    }
    public void Update(Guid id, decimal price, int stock)
        {
            var products = GetAll();
            var prod = products.Where(p => p.Id == id).FirstOrDefault();
            prod?.Price = price>=0? price: prod.Price;
            prod?.Stock = stock>1? stock: prod.Stock;
            Save(products);
        }
    public void Save(List<Product> products) => _repo.WriteAll(products);
    public decimal InventoryValue() => _repo.ReadAll().Sum(p => p.Price * p.Stock);
    }
}