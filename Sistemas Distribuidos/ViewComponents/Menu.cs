using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sistemas_Distribuidos.Models;

namespace Sistemas_Distribuidos.ViewComponents
{
    public class Menu : ViewComponent
    {
        // Carrega dinamicamente os controles de cadastro e login no topo da página
        // caso o usuário esteja logado, aparecerá seu nome e um botão de logout
        // caso contrário, aparecerá cadastro e login
        public async Task<IViewComponentResult> InvokeAsync()
        {
            string? userS = HttpContext.Session.GetString("user");

            // Se não estiver logado
            if (string.IsNullOrEmpty(userS)) return View("SemLogin");

            // Se estiver, obtém o usuário da sessão e retorna pra página
            UserModel? user = JsonConvert.DeserializeObject<UserModel>(userS);

            return View(user);
        }
    }
}
