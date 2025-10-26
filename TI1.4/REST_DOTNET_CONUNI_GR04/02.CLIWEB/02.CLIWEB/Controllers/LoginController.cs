using Microsoft.AspNetCore.Mvc;

namespace _02.CLIWEB.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string usuario, string clave)
        {
            if (usuario == "MONSTER" && clave == "MONSTER9")
            {
                HttpContext.Session.SetString("usuario", usuario);
                TempData["LoginSuccess"] = "true";
                return RedirectToAction("Index", "Conversion");
            }

            ViewBag.Error = "Usuario o contraseña incorrectos. Por favor, verifica tus credenciales.";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["LogoutSuccess"] = "true";
            return RedirectToAction("Index");
        }
    }
}
