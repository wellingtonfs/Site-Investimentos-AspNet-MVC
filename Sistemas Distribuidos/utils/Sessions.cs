using Newtonsoft.Json;
using Sistemas_Distribuidos.Models;
using System.Drawing;

namespace Sistemas_Distribuidos.utils
{
    public class Sessions : ISessions
    {
        private readonly IHttpContextAccessor _httpContext;

        // Injeção de dependências
        public Sessions(IHttpContextAccessor httpContextAccessor) {
            _httpContext = httpContextAccessor;
        }
        
        // Criar sessão do usuário quando ele entra no sistema
        public void CriarSession(UserModel user)
        {
            // Converte o modelo c# em uma string serializada em JSON
            string valor = JsonConvert.SerializeObject(user);

            // Salva na sessão
            _httpContext.HttpContext?.Session.SetString("user", valor);
        }

        // Criar um sessão genérica, dado uma chave e um valor
        public void CriarSession(string key, string value)
        {
            _httpContext.HttpContext?.Session.SetString(key, value);
        }

        // Obter a sessão do usuário, caso exista
        public UserModel? GetSession()
        {
            // Tenta buscar os dados serializados
            string? user = _httpContext.HttpContext?.Session.GetString("user");

            // Se não existir, retorna null
            if (string.IsNullOrEmpty(user)) return null;

            // Desserializa e retorna em modelo c#
            return JsonConvert.DeserializeObject<UserModel>(user);
        }

        // Obter sessão genérica de acordo com a chave passada
        public string? GetSession(string key)
        {
            // Tenta obter os dados
            string? result = _httpContext.HttpContext?.Session.GetString(key);

            // Se não existir, retorna null
            if (string.IsNullOrEmpty(result)) return null;

            return result;
        }

        // Remove todas as sessões do usupario
        public void RemoveSession()
        {
            _httpContext.HttpContext?.Session.Clear();
        }
    }
}
