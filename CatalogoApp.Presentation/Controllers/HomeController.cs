using CatalogoApp.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Catalogo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ItemService _itemService;

        public HomeController(ItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserRole = HttpContext.Session.GetString("UserRole");
            ViewBag.TotalJugadores = _itemService.ObtenerTodos().Count;
            ViewBag.TotalGoles = _itemService.ObtenerTodos().Sum(i => i.Goles);
            return View();
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
