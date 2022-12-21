using System.ComponentModel.DataAnnotations;

namespace Sistemas_Distribuidos.Models
{
    // Modelo dos dados de Login
    public class LoginModel
    {
        [Required(ErrorMessage = "E-mail ou apelido são necessários")]
        public string NickOrEmail { get; set; }
        [Required(ErrorMessage = "A senha é obrigatória")]
        public string Password { get; set; }
    }
}
