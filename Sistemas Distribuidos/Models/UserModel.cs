using Sistemas_Distribuidos.utils;
using System.ComponentModel.DataAnnotations;

namespace Sistemas_Distribuidos.Models
{
    // Modelo dos dados de Cadastro que serão salvos no banco de dados
    public class UserModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo de apelido (nick) é obrigatório")]
        public string Nick { get; set; }

        [Required(ErrorMessage = "O campo de e-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "O e-mail informado não é válido")]
        public string Email { get; set; }

        public bool ExistingEmail { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        public string Password { get; set; }

        public DateTime DataCadastro { get; set; }

        // Verifica se a senha é igual a senha que veio do login
        public bool SamePassword(LoginModel userLogin)
        {
            return Password == userLogin.Password;
        }

        // Verifica se o apelido é valido
        public static bool IsValidNick(string nick)
        {
            if (string.IsNullOrEmpty(nick)) return false;
            if (nick.Length < 3 || nick.Length > 30) return false;
            if (!RegexUtils.IsValidNick(nick)) return false;

            return true;
        }

        // Verifica se o email é válido
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return false;
            if (email.Length < 5) return false;
            if (!RegexUtils.IsEmail(email)) return false;

            return true;
        }

        // Verifica se a senha é válida
        public static bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return false;
            if (password.Length < 3 || password.Length > 30) return false;

            return true;
        }
    }
}
