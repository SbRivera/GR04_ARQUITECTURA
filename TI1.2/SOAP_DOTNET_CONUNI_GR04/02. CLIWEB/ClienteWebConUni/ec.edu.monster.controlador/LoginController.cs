using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ec.edu.monster.modelo;
using System;

namespace ec.edu.monster.controlador
{
    public class LoginController : Controller
    {
        private const string UsuarioValido = "MONSTER";
        private const string PasswordValida = "MONSTER9";
        private const string SessionKeyUsuario = "usuario";

        [HttpGet]
        public IActionResult Index()
        {
            var usuario = HttpContext.Session.GetString(SessionKeyUsuario);
            if (!string.IsNullOrEmpty(usuario))
            {
                return RedirectToAction("Index", "Conversion");
            }

            return View(new LoginModel());
        }

        [HttpPost]
        public IActionResult Index(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Error = "Por favor completa ambos campos.";
                return View(model);
            }

            if (CredencialesSonValidas(model.Usuario, model.Password))
            {
                HttpContext.Session.SetString(SessionKeyUsuario, UsuarioValido);
                return RedirectToAction("Index", "Conversion");
            }

            model.Error = "Credenciales incorrectas. Inténtalo nuevamente.";
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        private static bool CredencialesSonValidas(string usuarioIngresado, string passwordIngresada)
        {
            return string.Equals(usuarioIngresado?.Trim(), UsuarioValido, StringComparison.OrdinalIgnoreCase)
                   && string.Equals(passwordIngresada, PasswordValida, StringComparison.Ordinal);
        }


    }
}