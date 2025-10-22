using Microsoft.AspNetCore.Mvc;

namespace WS_ConUni_REST_DOTNET.Controllers
{
    public class ConversionRequest
    {
        public string? Type { get; set; }
        public double Value { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class ConUni : ControllerBase
    {

        // POST /ConUni/ConUni
        // Body (raw JSON): { "type": "cm_to_inch", "value": 123.45 }
        // Tipos aceptados (aliases):
        // - cm -> cm_to_inch, cm2in, cm-in
        // - inch -> inch_to_cm, in2cm, in-cm
        // - f -> fahrenheit_to_celsius, f2c, f_to_c
        // - c -> celsius_to_fahrenheit, c2f, c_to_f
        // - kg -> kg_to_lb, kg2lb
        // - lb -> lb_to_kg, lb2kg
        [HttpPost("ConUni", Name = "ConUni")]
        [Consumes("application/json")]
        public ActionResult<object> POST([FromBody] ConversionRequest? request)
        {
            if (request is null)
                return BadRequest(new { error = "Cuerpo JSON inválido o vacío. Envíe { \"type\": \"...\", \"value\": 123.45 }" });

            var type = request.Type;
            var value = request.Value;

            var converter = new WS_ConUni();

            if (string.IsNullOrWhiteSpace(type))
                return BadRequest(new { error = "Falta el parámetro 'type' en el cuerpo." });

            switch (type.Trim().ToLowerInvariant())
            {
                // Centímetros -> Pulgadas
                case "cm_to_inch":
                case "cm2in":
                case "cm-in":
                case "cm":
                    {
                        double inchesVal = converter.cm_to_inch(value);
                        return Ok(new { type, input = $"{value:F2} cm", output = $"{inchesVal:F2} in" });
                    }

                // Pulgadas -> Centímetros
                case "inch_to_cm":
                case "in2cm":
                case "in-cm":
                case "inch":
                    {
                        double cmVal = converter.inch_to_cm(value);
                        return Ok(new { type, input = $"{value:F2} in", output = $"{cmVal:F2} cm" });
                    }

                // Fahrenheit -> Celsius
                case "f_to_c":
                case "f2c":
                case "fahrenheit":
                case "f":
                    {
                        double cVal = converter.fahrenheit_to_celsius(value);
                        return Ok(new { type, input = $"{value:F2} °F", output = $"{cVal:F2} °C" });
                    }

                // Celsius -> Fahrenheit
                case "c_to_f":
                case "c2f":
                case "celsius":
                case "c":
                    {
                        double fVal = converter.celsius_to_fahrenheit(value);
                        return Ok(new { type, input = $"{value:F2} °C", output = $"{fVal:F2} °F" });
                    }

                // Kilogramos -> Libras
                case "kg_to_lb":
                case "kg2lb":
                case "kg":
                    {
                        double lbVal = converter.kg_to_lb(value);
                        return Ok(new { type, input = $"{value:F2} kg", output = $"{lbVal:F2} lb" });
                    }

                // Libras -> Kilogramos
                case "lb_to_kg":
                case "lb2kg":
                case "lb":
                case "libras":
                    {
                        double kgVal = converter.lb_to_kg(value);
                        return Ok(new { type, input = $"{value:F2} lb", output = $"{kgVal:F2} kg" });
                    }

                default:
                    return BadRequest(new
                    {
                        error = $"Tipo de conversión '{type}' no soportado.",
                        supported = new[] { "cm_to_inch", "inch_to_cm", "f_to_c", "c_to_f", "kg_to_lb", "lb_to_kg" }
                    });
            }
        }

    }
}