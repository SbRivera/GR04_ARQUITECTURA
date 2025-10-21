using System.ComponentModel.DataAnnotations;

namespace ec.edu.monster.modelo
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Ingresa tu usuario.")]
        public string Usuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "Ingresa tu contraseña.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public string? Error { get; set; }
    }
}