using CatalogoApp.Domain.Interfaces;
using CatalogoApp.Domain.Models;

namespace CatalogoApp.Application.Services
{
    public class ReviewService
    {
        private readonly IReviewRepository _repo;

        public ReviewService(IReviewRepository repo)
        {
            _repo = repo;
        }

        public List<Review> ObtenerTodos()
        {
            return _repo.ObtenerTodos();
        }

        public List<Review> ObtenerPorItem(int itemId)
        {
            return _repo.ObtenerPorItem(itemId);
        }

        public void Agregar(int itemId, int userId, string userName, int calificacion, string comentario)
        {
            calificacion = Math.Clamp(calificacion, 1, 5);

            _repo.Agregar(new Review
            {
                ItemId = itemId,
                UserId = userId,
                UserName = userName,
                Calificacion = calificacion,
                Comentario = comentario.Trim()
            });
        }
    }
}
