using _02.CLIWEB.ec.edu.monster.modelo;
using _02.CLIWEB.ec.edu.monster.servicio;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _02.CLIWEB.ec.edu.monster.controler
{
    public class ConversionController : Controller
    {
        private readonly ConUniService _service;

        // Mapeo de códigos de la VISTA -> SERVICIO
        private static readonly Dictionary<string, string> TipoVistaAService = new()
        {
            ["cmToIn"] = "cm",
            ["inToCm"] = "inch",
            ["cToF"] = "c",
            ["fToC"] = "f",
            ["kgToLb"] = "kg",
            ["lbToKg"] = "lb"
        };

        public ConversionController(ConUniService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("usuario") == null)
                return RedirectToAction("Index", "Login");

            // Vista unificada vacía (sin resultado aún)
            return View(new Resultado());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(string tipo, double? valor)
        {
            if (HttpContext.Session.GetString("usuario") == null)
                return RedirectToAction("Index", "Login");

            if (string.IsNullOrWhiteSpace(tipo) || !valor.HasValue)
            {
                ViewBag.Error = "Debe seleccionar un tipo y un valor válido.";
                // Rehidratación mínima en la vista
                ViewData["viewTipo"] = tipo ?? "";
                ViewData["valor"] = valor?.ToString() ?? "";
                return View(new Resultado());
            }

            var tipoServicio = TipoVistaAService.TryGetValue(tipo, out var t) ? t : tipo;

            try
            {
                Resultado resultado = await _service.ConvertirAsync(tipoServicio, valor.Value);

                // Guardamos info para rehidratar selects y mostrar detalles
                ViewData["viewTipo"] = tipo;          // cmToIn/inToCm/... (código de la vista)
                ViewData["valor"] = valor.Value;   // para mostrar el valor ingresado
                return View(resultado);               // MISMA VISTA con resultado
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error al procesar la conversión: {ex.Message}";
                ViewData["viewTipo"] = tipo;
                ViewData["valor"] = valor.Value;
                return View(new Resultado());         // devolvemos vista con error
            }
        }

        // OPCIONAL: si quieres conservar la ruta /Conversion/Convertir usada antes:
        [HttpPost]
        [ValidateAntiForgeryToken]
        public Task<IActionResult> Convertir(string tipo, double? valor)
        {
            // Reutilizamos la lógica de Index(POST) para no duplicar código:
            return Index(tipo, valor);
        }
    }
}
