using CatalogoApp.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Catalogo.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserService _userService;

        public AccountController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Completa todos los campos.";
                return View();
            }

            var user = _userService.Login(email.Trim(), password);
            if (user == null)
            {
                ViewBag.Error = "Correo o contraseña incorrectos.";
                return View();
            }

            GuardarSesion(user.Id, user.Nombre, user.Role);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        public IActionResult Register(string nombre, string email, string password)
        {
            var result = _userService.Registrar(nombre, email, password);
            if (!result.Ok || result.User == null)
            {
                ViewBag.Error = result.Mensaje;
                return View();
            }

            GuardarSesion(result.User.Id, result.User.Nombre, result.User.Role);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId != null)
                _userService.RegistrarSalida(userId.Value);

            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        private void GuardarSesion(int userId, string userName, string role)
        {
            HttpContext.Session.SetInt32("UserId", userId);
            HttpContext.Session.SetString("UserName", userName);
            HttpContext.Session.SetString("UserRole", role);
        }
    }
}
