using CatalogoApp.Domain.Models;

namespace CatalogoApp.Domain.Interfaces
{
    public interface IUserRepository
    {
        List<User> ObtenerTodos();
        User? ObtenerPorId(int id);
        User? ObtenerPorEmail(string email);
        User Registrar(User user);
        void Actualizar(User user);
    }
}
