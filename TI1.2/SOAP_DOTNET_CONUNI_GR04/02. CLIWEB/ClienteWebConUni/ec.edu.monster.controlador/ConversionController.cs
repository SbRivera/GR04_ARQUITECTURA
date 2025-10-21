using Microsoft.AspNetCore.Mvc;
using WSConUniConsumer.Models;
using WSConUniConsumer.Services;
using System.Collections.Generic;
using System.Globalization;

namespace WSConUniConsumer.Controllers
{
    public class ConversionController : Controller
    {
        private static readonly HashSet<string> PermiteNegativos = new() { "cToF", "fToC" };

        [HttpGet]
        public IActionResult Index()
        {
            return View(new ConversionViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(ConversionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Error = "Verifica los datos proporcionados.";
                return View(model);
            }

            if (!float.TryParse(model.Valor, NumberStyles.Float, CultureInfo.InvariantCulture, out var valorNumerico))
            {
                model.Error = "El valor ingresado no es numérico.";
                return View(model);
            }

            if (valorNumerico < 0 && !PermiteNegativos.Contains(model.Tipo))
            {
                model.Error = "No se permiten valores negativos para esta conversión.";
                return View(model);
            }

            try
            {
                using var proxy = new WSConUniProxy();
                model.Resultado = await proxy.ConvertAsync(model.Tipo, valorNumerico);
            }
            catch (System.Exception ex)
            {
                model.Error = $"No fue posible completar la operación. Detalles: {ex.Message}";
            }

            return View(model);
        }
    }
}