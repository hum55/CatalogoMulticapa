using System.Text.Json;
using CatalogoApp.Domain.Interfaces;
using CatalogoApp.Domain.Models;

namespace CatalogoApp.Infrastructure.Repositories
{
    public class JsonItemRepository : IItemRepository
    {
        private readonly string _filePath;
        private readonly object _lock = new();
        private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

        public JsonItemRepository(string filePath)
        {
            _filePath = filePath;
            var carpeta = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(carpeta))
                Directory.CreateDirectory(carpeta);
        }

        public List<Item> ObtenerTodos()
        {
            lock (_lock)
            {
                if (!File.Exists(_filePath))
                    return new List<Item>();

                var json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<Item>>(json) ?? new List<Item>();
            }
        }

        public Item? ObtenerPorId(int id)
        {
            return ObtenerTodos().FirstOrDefault(i => i.Id == id);
        }

        public void Agregar(Item item)
        {
            lock (_lock)
            {
                var items = LeerSinLock();
                item.Id = items.Count > 0 ? items.Max(i => i.Id) + 1 : 1;
                items.Add(item);
                GuardarSinLock(items);
            }
        }

        public void Actualizar(Item item)
        {
            lock (_lock)
            {
                var items = LeerSinLock();
                var index = items.FindIndex(i => i.Id == item.Id);
                if (index >= 0)
                {
                    items[index] = item;
                    GuardarSinLock(items);
                }
            }
        }

        public void Eliminar(int id)
        {
            lock (_lock)
            {
                var items = LeerSinLock();
                var aEliminar = items.FirstOrDefault(i => i.Id == id);
                if (aEliminar != null)
                {
                    items.Remove(aEliminar);
                    GuardarSinLock(items);
                }
            }
        }

        private List<Item> LeerSinLock()
        {
            if (!File.Exists(_filePath))
                return new List<Item>();
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Item>>(json) ?? new List<Item>();
        }

        private void GuardarSinLock(List<Item> items)
        {
            var json = JsonSerializer.Serialize(items, _options);
            File.WriteAllText(_filePath, json);
        }
    }
}
