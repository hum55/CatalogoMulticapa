using CatalogoApp.Application.Helpers;
using CatalogoApp.Domain.Interfaces;
using CatalogoApp.Domain.Models;

namespace CatalogoApp.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo)
        {
            _repo = repo;
        }

        public User? Login(string email, string password)
        {
            var user = _repo.ObtenerPorEmail(email);
            if (user == null)
                return null;

            bool valid;
            if (PasswordHasher.IsHashed(user.Password))
            {
                valid = PasswordHasher.Verify(password, user.Password);
            }
            else
            {
                valid = user.Password == password;
                if (valid)
                {
                    user.Password = PasswordHasher.Hash(password);
                }
            }

            if (!valid)
                return null;

            user.UltimoLogin = DateTime.Now;
            user.VecesIngreso++;
            _repo.Actualizar(user);
            return user;
        }

        public (bool Ok, string Mensaje, User? User) Registrar(string nombre, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(nombre) || nombre.Trim().Length < 2)
                return (false, "El nombre debe tener al menos 2 caracteres.", null);

            if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
                return (false, "Correo no valido.", null);

            if (string.IsNullOrWhiteSpace(password) || password.Length < 4)
                return (false, "La contraseña debe tener al menos 4 caracteres.", null);

            if (_repo.ObtenerPorEmail(email) != null)
                return (false, "Ese correo ya esta registrado.", null);

            var user = new User
            {
                Nombre = nombre.Trim(),
                Email = email.Trim().ToLowerInvariant(),
                Password = PasswordHasher.Hash(password),
                Role = "User",
                UltimoLogin = DateTime.Now,
                VecesIngreso = 1
            };

            return (true, "", _repo.Registrar(user));
        }

        public void RegistrarSalida(int userId)
        {
            var user = _repo.ObtenerPorId(userId);
            if (user == null)
                return;

            user.UltimoLogout = DateTime.Now;
            _repo.Actualizar(user);
        }

        public User? ObtenerPorId(int id) => _repo.ObtenerPorId(id);

        public bool EsAdmin(int userId)
        {
            var user = _repo.ObtenerPorId(userId);
            return user?.Role == "Admin";
        }
    }
}
