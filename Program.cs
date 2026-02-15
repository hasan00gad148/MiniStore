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
                        break;

                    case "4": // add product
                        break;

                    case "5": // update stock
                        break;

                    case "6": // add to cart
                        break;

                    case "7": // view cart

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