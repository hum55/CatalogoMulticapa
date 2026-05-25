using System.Text.Json;
using CatalogoApp.Domain.Interfaces;
using CatalogoApp.Domain.Models;

namespace CatalogoApp.Infrastructure.Repositories
{
    public class JsonReviewRepository : IReviewRepository
    {
        private readonly string _filePath;
        private readonly object _lock = new();
        private readonly JsonSerializerOptions _options = new() { WriteIndented = true };

        public JsonReviewRepository(string filePath)
        {
            _filePath = filePath;
            var carpeta = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrEmpty(carpeta))
                Directory.CreateDirectory(carpeta);
        }

        public List<Review> ObtenerTodos()
        {
            lock (_lock)
            {
                return LeerSinLock();
            }
        }

        public List<Review> ObtenerPorItem(int itemId)
        {
            return ObtenerTodos()
                .Where(r => r.ItemId == itemId)
                .OrderByDescending(r => r.Fecha)
                .ToList();
        }

        public void Agregar(Review review)
        {
            lock (_lock)
            {
                var reviews = LeerSinLock();
                review.Id = reviews.Count > 0 ? reviews.Max(r => r.Id) + 1 : 1;
                review.Fecha = DateTime.Now;
                reviews.Add(review);
                GuardarSinLock(reviews);
            }
        }

        private List<Review> LeerSinLock()
        {
            if (!File.Exists(_filePath))
                return new List<Review>();
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<Review>>(json) ?? new List<Review>();
        }

        private void GuardarSinLock(List<Review> reviews)
        {
            var json = JsonSerializer.Serialize(reviews, _options);
            File.WriteAllText(_filePath, json);
        }
    }
}
