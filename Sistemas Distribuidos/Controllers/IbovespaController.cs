using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SendGrid;
using SendGrid.Helpers.Mail;
using Sistemas_Distribuidos.Models;
using Sistemas_Distribuidos.Models.Hg;
using Sistemas_Distribuidos.Models.Yahoo;
using Sistemas_Distribuidos.Repositorio;
using Sistemas_Distribuidos.Services;
using System.Diagnostics;

namespace Sistemas_Distribuidos.Controllers
{
    public class IbovespaController : Controller
    {
        private readonly IMemoryCache _cache;

        // Injetar dependências
        public IbovespaController(IMemoryCache cache)
        {
            _cache = cache;
        }

        // Página '/'
        public IActionResult Index()
        {
            return View();
        }

        // Rota '/Data?tipo=ibovespa&intervalo=30m&range=2d'
        public async Task<IActionResult> Data(string tipo, string? intervalo, string? range)
        {
            // Verificando se o valor dos parâmetros são válidos
            if (tipo != "ibovespa") return Json(null); //BVSP
            if (intervalo != null && !YahooAPI.intervalosAceitos.Contains(intervalo)) return Json(null);
            if (range != null && !YahooAPI.rangesAceitos.Contains(range)) return Json(null);

            // Caso intervalo e range forem nulos, o valor padrão é usado
            intervalo ??= "30m";
            range ??= "2d";

            // Colocando cache para não sobrecarregar a api
            YahooModelIbovespa? result = await _cache.GetOrCreateAsync($"historicoIbovespa{intervalo}{range}", async (entry) =>
            {
                // Cache de 15 minutos pois é o tempo médio de atualização das APIs usadas
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15);

                // Retorna o histórico de dados da API
                return await YahooAPI.ObterHistorioIbovespa(intervalo, range);
            });

            // Retorna pra página o resultado
            return Json(result);
        }

        // Não utilizado
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}