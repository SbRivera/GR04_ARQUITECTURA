using System.ComponentModel.DataAnnotations;

namespace WSConUniConsumer.Models
{
    public class ConversionViewModel
    {
        [Required(ErrorMessage = "Ingresa un valor numérico.")]
        [Display(Name = "Valor")]
        public string Valor { get; set; } = string.Empty;

        [Required(ErrorMessage = "Selecciona una operación.")]
        public string Tipo { get; set; } = string.Empty;

        public string? Resultado { get; set; }
        public string? Error { get; set; }
    }
}