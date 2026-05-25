using CatalogoApp.Domain.Interfaces;
using CatalogoApp.Domain.Models;

namespace CatalogoApp.Application.Services
{
    public class ItemService
    {
        private readonly IItemRepository _repo;

        public ItemService(IItemRepository repo)
        {
            _repo = repo;
        }

        public List<Item> ObtenerTodos()
        {
            return _repo.ObtenerTodos();
        }

        public Item? ObtenerPorId(int id)
        {
            return _repo.ObtenerPorId(id);
        }

        public void Agregar(Item item)
        {
            if (string.IsNullOrWhiteSpace(item.Genero))
                item.Genero = "Futbol";
            _repo.Agregar(item);
        }

        public void Actualizar(Item item)
        {
            _repo.Actualizar(item);
        }

        public void Eliminar(int id)
        {
            _repo.Eliminar(id);
        }

        public List<Item> Buscar(string? query, string? genero, string? posicion, string? equipo)
        {
            var items = _repo.ObtenerTodos().AsEnumerable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                var q = query.Trim().ToLowerInvariant();
                items = items.Where(i =>
                    i.Nombre.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    i.Equipo.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    i.Pais.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                    i.Clubes.Contains(q, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(genero))
                items = items.Where(i => i.Genero.Equals(genero, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(posicion))
                items = items.Where(i => i.Posicion.Contains(posicion, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(equipo))
                items = items.Where(i => i.Equipo.Equals(equipo, StringComparison.OrdinalIgnoreCase));

            return items.ToList();
        }

        public List<string> ObtenerGeneros()
        {
            return _repo.ObtenerTodos()
                .Select(i => i.Genero)
                .Where(g => !string.IsNullOrWhiteSpace(g))
                .Distinct()
                .OrderBy(g => g)
                .ToList();
        }

        public List<string> ObtenerEquipos()
        {
            return _repo.ObtenerTodos()
                .Select(i => i.Equipo)
                .Where(e => !string.IsNullOrWhiteSpace(e))
                .Distinct()
                .OrderBy(e => e)
                .ToList();
        }
    }
}
