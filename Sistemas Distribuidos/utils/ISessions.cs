using Sistemas_Distribuidos.Models;

namespace Sistemas_Distribuidos.utils
{
    public interface ISessions
    {
        // Métodos que podem ser acessados para configurar a sessão do usuário

        public void CriarSession(UserModel user);
        public void CriarSession(string key, string value);
        public void RemoveSession();
        public UserModel? GetSession();
        public string? GetSession(string key);
    }
}
