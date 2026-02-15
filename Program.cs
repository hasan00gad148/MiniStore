// See https://aka.ms/new-console-template for more information
//using MyLibrary;
//Class1.Main(); 
using MiniStore.Models;
using MiniStore.Services;


namespace MiniStore
{
    public class MiniStore
    {
        public static void Main(string[] args)
        {
            
            var dataPath = Path.Combine(Directory.GetCurrentDirectory(),"../../../", "Data");

            var userService = new UserService(Path.Combine(dataPath, "users.json"));
            var productService = new ProductService(Path.Combine(dataPath, "products.json"));
            var cartService = new CartService(Path.Combine(dataPath, "carts.json"), productService);

            while (true)
            {
                Console.WriteLine("\nAvailable Commands:");
                Console.WriteLine("1. list users");
                Console.WriteLine("2. add user");
                Console.WriteLine("3. list products");
                Console.WriteLine("4. add product");
                Console.WriteLine("5. update stock");
                Console.WriteLine("6. add to cart");
                Console.WriteLine("7. view cart");
                Console.WriteLine("8. checkout");
                Console.WriteLine("9. list orders");
                Console.WriteLine("10. show order");
                Console.WriteLine("0. exit");
                Console.Write("Enter command number: ");

                var input = Console.ReadLine()?.Trim();

                switch (input)
                {
                    case "1": // list users
                        var users = userService.GetAll();
                        Console.WriteLine("\nUsers:");
                        foreach (var u in users)
                            Console.WriteLine($"- {u.Name} ({u.Email}) [{u.Id}]");
                        break;

                    case "2": // add user
                        Console.Write("Name: ");
                        var name = Console.ReadLine();
                        Console.Write("Email: ");
                        var email = Console.ReadLine();
                        if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(email))
                            userService.Add(name, email);
                        break;

                    case "3": // list products
                        var products = productService.GetAll();
                        Console.WriteLine("\nProducts:");
                        foreach (var p in products)
                            Console.WriteLine($"- {p.Name} | Price: {p.Price:C} | Stock: {p.Stock} | Id: {p.Id} | Total: {p.Price * p.Stock}");
                        break;

                    case "4": // add product
                        Console.Write("Name: ");
                        var pname = Console.ReadLine();
                        Console.Write("Price: ");
                        var pprice = decimal.TryParse(Console.ReadLine(), out var pr) ? pr : 0;
                        Console.Write("Stock: ");
                        var pstock = int.TryParse(Console.ReadLine(), out var st) ? st : -1;
                        if (!string.IsNullOrEmpty(pname))
                            productService.Add(pname, pprice, pstock);
                        break;

                    case "5": // update stock
                        Console.Write("Product Id: ");
                        var pid = Console.ReadLine();
                        if (Guid.TryParse(pid, out var guidPid))
                        {
                            var prod = productService.GetById(guidPid);
                            if (prod != null)
                            {
                                Console.Write($"new Price for {prod.Name}: ");
                                decimal Price = decimal.TryParse(Console.ReadLine(), out var npr) ? npr : prod.Price;
                                Console.Write($"new Stock for {prod.Name}: ");
                                int Stock = int.TryParse(Console.ReadLine(), out var nst) ? nst : prod.Stock;

                                productService.Update(guidPid, Price, Stock);
                                Console.WriteLine("✅ Stock updated.");
                            }
                            else Console.WriteLine("❌ Product not found.");
                        }
                        break;

                    case "6": // add to cart
                        Console.Write("User Id: ");
                        var uid = Console.ReadLine();
                        Console.Write("Product Id: ");
                        var prid = Console.ReadLine();
                        Console.Write("Quantity: ");
                        var qty = int.TryParse(Console.ReadLine(), out var q) ? q : 0;
                        if (Guid.TryParse(uid, out var guidUid) && Guid.TryParse(prid, out var guidPr))
                            cartService.AddToCart(guidUid, guidPr, qty);
                        break;

                    case "7": // view cart
                        Console.Write("User Id: ");
                        var vu = Console.ReadLine();
                        if (Guid.TryParse(vu, out var guidVu))
                        {
                            var cart = cartService.GetCart(guidVu);
                            Console.WriteLine($"\nCart (Total: {cart.Total:C}):");
                            foreach (var item in cart.items)
                                Console.WriteLine($"- {item.ProductName} | {item.Quantity} x {item.ProductPrice:C} = {item.LineTotal:C}");
                        }
                        break;

                    case "8": // checkout
                        break;

                    case "9": // list orders
                        break;

                    case "10": // show order
                        break;

                    case "0": // exit
                        return;

                    default:
                        Console.WriteLine("❌ Unknown command.");
                        break;
                }
            }
        }
    }
}