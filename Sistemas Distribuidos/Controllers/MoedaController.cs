using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Sistemas_Distribuidos.Models.Hg;
using Sistemas_Distribuidos.Repositorio;
using Sistemas_Distribuidos.Services;

namespace Sistemas_Distribuidos.Controllers
{
    public class MoedaController : Controller
    {
        private IHgRepositorio _hgRepositorio;
        private readonly IMemoryCache _cache;

        // Injeção de dependencias
        public MoedaController(IHgRepositorio hgRepositorio, IMemoryCache cache)
        {
            _cache = cache;
            _hgRepositorio = hgRepositorio;
        }

        // Página '/'
        public async Task<IActionResult> Index()
        {
            // Busca a cotação das moedas na API Hg Finance
            List<MoedaModel>? result = await HgAPI.ObterCotacaoMoedas(_cache);

            // Retorna o resultado
            return View(result);
        }

        // Página '/Data?moeda=USD'
        public ActionResult Data(string moeda)
        {
            // Verificando se a moeda recebida é válida
            if (!MoedaModel.MoedasAceitas().Contains(moeda)) return Json(null);

            // Colocando cache para não sobrecarregar o banco de dados
            List<MoedaModel>? result = _cache.GetOrCreate("moedaDB" + moeda, entry =>
            {
                // Cache de 15 minutos
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15);

                // Retorna o resultado
                return _hgRepositorio.ObterHistoricoMoeda(moeda);
            });

            // Envia o resultado pra página
            return Json(result);
        }
    }
}
