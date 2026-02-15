using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniStore.Models;
using MiniStore.Repos;

namespace MiniStore.Services
{
    public class UserService
    {
          private JsonRepo<User> _repo;

    public UserService(string path) => _repo = new JsonRepo<User>(path);

    public List<User> GetAll() => _repo.ReadAll().OrderBy(u => u.Name).ToList();

    public User? GetByEmail(string email) =>
        _repo.ReadAll().FirstOrDefault(u =>
            string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase));

    public void Add(string name, string email)
    {
        var users = _repo.ReadAll();
        if (users.Any(u => string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase)))
        {
            Console.WriteLine("❌ Email already exists.");
            return;
        }

        users.Add(new User { Name = name, Email = email });
        _repo.WriteAll(users);
        Console.WriteLine("✅ User added.");
    }
    }
}
