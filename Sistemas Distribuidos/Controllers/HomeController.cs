using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
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
    public class HomeController : Controller
    {
        private readonly IMemoryCache _cache;
        private readonly IHttpContextAccessor _contexto;
        private readonly ILogger _logger;

        // Injetar dependências
        public HomeController(ILogger<HomeController> logger, IMemoryCache cache, IHttpContextAccessor contexto) {
            _logger = logger;
            _cache = cache;
            _contexto = contexto;
        }

        // Página '/'
        public IActionResult Index()
        {
            // Obtém o IP dos clientes e coloca no Log
            string? ip = _contexto.HttpContext?.Connection.RemoteIpAddress?.ToString();

            // Caso não for nulo, é salvo
            if (ip != null) {
                _logger.LogInformation($"Cliente conectado: {ip}");

                UpdateTask.AddToLogFile($"[{DateTime.Now}] User IP {ip}");
            }

            // Carrega a página para o cliente
            return View();
        }

        // Rota '/Pesquisar?query=Empresa'
        public async Task<IActionResult> Pesquisar(string query)
        {
            // Se a query for inválida, retorna null
            if (query == null || query.Length == 0) return Json(null);

            // Busca a resposta da API
            JObject? result = await YahooAPI.Pesquisar(_cache, query);

            // Se der problema, retorna null
            if (result == null) return Json(null);

            // Retorna o JSON de resposta pra página
            return Content(result.ToString(), "application/json");
        }

        // Rota '/ObterAcao?tag=Symbol'
        public async Task<IActionResult> ObterAcao(string tag)
        {
            // Caso a tag for inválida, retorna null
            if (tag == null || tag.Length == 0) return Json(null);

            // Busca a ação da API e retorna como uma lista contendo: [nomeMoeda, valor]
            List<string>? result = await YahooAPI.ObterAcaoByTag(tag);

            // Se for null, retorna null
            if (result == null) return Json(null);

            // Tenta converter o result de dolar para real caso for dolar
            if (result[0] == "USD" && result.Count == 2)
            {
                // Busca a cotação atual do dolar para realizar a conversão
                float convert = HgAPI.ConverterDolarPraReal(result[1]) ?? -1;

                // Se convert for positivo, significa que deu certo
                result[0] = (convert < 0) ? result[0] : "BRL";
                result[1] = (convert < 0) ? result[1] : convert.ToString();
            }

            // Retorna o resultado pra página
            return Json(result);
        }
    }
}