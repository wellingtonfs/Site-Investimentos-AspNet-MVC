using System.ComponentModel.DataAnnotations;

namespace Sistemas_Distribuidos.Models
{

    // Modelo dos dados de recuperação de senha
    public class RecSenhaModel
    {
        [Required(ErrorMessage = "E-mail ou apelido são necessários")]
        public string NickOrEmail { get; set; }
    }
}
