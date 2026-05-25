using System.Text.Json;
using CatalogoApp.Domain.Interfaces;
using CatalogoApp.Domain.Models;

namespace CatalogoApp.Infrastructure.Repositories
{
    public class JsonUserRepository : IUserRepository
    {
        private readonly string _filePath;
        private readonly object _lock = new();
        private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

        public JsonUserRepository(string filePath)
        {
            _filePath = filePath;
            var carpeta = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(carpeta))
                Directory.CreateDirectory(carpeta);
        }

        public List<User> ObtenerTodos()
        {
            lock (_lock)
            {
                return LeerSinLock();
            }
        }

        public User? ObtenerPorId(int id)
        {
            return ObtenerTodos().FirstOrDefault(u => u.Id == id);
        }

        public User? ObtenerPorEmail(string email)
        {
            return ObtenerTodos()
                .FirstOrDefault(u => string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase));
        }

        public User Registrar(User user)
        {
            lock (_lock)
            {
                var users = LeerSinLock();
                user.Id = users.Count > 0 ? users.Max(u => u.Id) + 1 : 1;
                users.Add(user);
                GuardarSinLock(users);
                return user;
            }
        }

        public void Actualizar(User user)
        {
            lock (_lock)
            {
                var users = LeerSinLock();
                var index = users.FindIndex(u => u.Id == user.Id);
                if (index >= 0)
                {
                    users[index] = user;
                    GuardarSinLock(users);
                }
            }
        }

        private List<User> LeerSinLock()
        {
            if (!File.Exists(_filePath))
                return new List<User>();
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<User>>(json) ?? new List<User>();
        }

        private void GuardarSinLock(List<User> users)
        {
            var json = JsonSerializer.Serialize(users, _options);
            File.WriteAllText(_filePath, json);
        }
    }
}
