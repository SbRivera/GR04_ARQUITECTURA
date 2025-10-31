using _02.CLIWEB.ec.edu.monster.servicio;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace _02.CLIWEB.ec.edu.monster.controler
{
    public class ConversionController : Controller
    {
        private readonly ConUniService _service;

        public ConversionController(ConUniService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("usuario") == null)
                return RedirectToAction("Index", "Login");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Convertir(string tipo, double valor)
        {
            var resultado = await _service.ConvertirAsync(tipo, valor);
            return View("Resultado", resultado);
        }
    }
}
