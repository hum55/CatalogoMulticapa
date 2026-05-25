using CatalogoApp.Application.Services;
using CatalogoApp.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Catalogo.Controllers
{
    public class CatalogoController : Controller
    {
        private readonly ItemService _itemService;
        private readonly ReviewService _reviewService;

        public CatalogoController(ItemService itemService, ReviewService reviewService)
        {
            _itemService = itemService;
            _reviewService = reviewService;
        }

        [HttpGet]
        public IActionResult Index(string? q, string? genero, string? equipo)
        {
            var items = _itemService.Buscar(q, genero, null, equipo);
            ViewBag.Reviews = _reviewService.ObtenerTodos();
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserRole = HttpContext.Session.GetString("UserRole");
            ViewBag.Query = q;
            ViewBag.Genero = genero;
            ViewBag.Equipo = equipo;
            ViewBag.Generos = _itemService.ObtenerGeneros();
            ViewBag.Equipos = _itemService.ObtenerEquipos();
            return View(items);
        }

        [HttpGet]
        public IActionResult Agregar()
        {
            if (!EstaLogueado())
                return RedirectToAction("Login", "Account");
            return View(new Item());
        }

        [HttpPost]
        public IActionResult Agregar(Item item)
        {
            if (!EstaLogueado())
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                _itemService.Agregar(item);
                TempData["Success"] = "Jugador agregado correctamente.";
                return RedirectToAction("Index");
            }
            return View(item);
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            if (!EstaLogueado())
                return RedirectToAction("Login", "Account");

            var item = _itemService.ObtenerPorId(id);
            if (item == null) return NotFound();

            return View(item);
        }

        [HttpPost]
        public IActionResult Editar(Item item)
        {
            if (!EstaLogueado())
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                _itemService.Actualizar(item);
                TempData["Success"] = "Jugador actualizado correctamente.";
                return RedirectToAction("Detalle", new { id = item.Id });
            }
            return View(item);
        }

        [HttpPost]
        public IActionResult Eliminar(int id)
        {
            if (!EstaLogueado())
                return RedirectToAction("Login", "Account");

            _itemService.Eliminar(id);
            TempData["Success"] = "Jugador eliminado.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Detalle(int id)
        {
            var item = _itemService.ObtenerPorId(id);
            if (item == null) return NotFound();

            ViewBag.Reviews = _reviewService.ObtenerPorItem(id);
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
            ViewBag.UserRole = HttpContext.Session.GetString("UserRole");
            return View(item);
        }

        [HttpPost]
        public IActionResult AgregarReview(int itemId, int calificacion, string comentario)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var userName = HttpContext.Session.GetString("UserName");

            if (userId == null || string.IsNullOrWhiteSpace(userName))
                return RedirectToAction("Login", "Account");

            if (!string.IsNullOrWhiteSpace(comentario))
                _reviewService.Agregar(itemId, userId.Value, userName, calificacion, comentario);

            return RedirectToAction("Detalle", new { id = itemId });
        }

        private bool EstaLogueado()
        {
            return HttpContext.Session.GetInt32("UserId") != null;
        }
    }
}
