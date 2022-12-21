using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Sistemas_Distribuidos.Models.Hg;
using Sistemas_Distribuidos.Repositorio;
using Sistemas_Distribuidos.Services;

namespace Sistemas_Distribuidos.Controllers
{
    public class BitcoinController : Controller
    {
        private IHgRepositorio _hgRepositorio;
        private readonly IMemoryCache _cache;

        // Injetar dependências
        public BitcoinController(IHgRepositorio hgRepositorio, IMemoryCache cache)
        {
            _cache = cache;
            _hgRepositorio = hgRepositorio;
        }

        // Página '/'
        public async Task<IActionResult> Index()
        {
            // Obtém as cotações da API e retorna como lista para a página
            List<CorretoraModel>? result = await HgAPI.ObterCotacaoCorretoras(_cache);

            // Retorna o resultado para a página
            return View(result);
        }
    }
}
