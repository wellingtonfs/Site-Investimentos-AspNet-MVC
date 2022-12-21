using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;
using Sistemas_Distribuidos.Data;
using Sistemas_Distribuidos.Models.Hg;
using Sistemas_Distribuidos.Models.Yahoo;
using System.Diagnostics;

namespace Sistemas_Distribuidos.Services
{
    public class YahooAPI
    {
        static readonly HttpClient client = new HttpClient();

        // Configura as listas de parâmetros aceitos pelo site

        public static readonly List<string> intervalosAceitos = new List<string> {
            "30m", "1h", "1d", "5d"
        };

        public static readonly List<string> rangesAceitos = new List<string> {
            "1d", "2d", "5d", "1mo", "max"
        };

        // Exemplo de empresas que são mostrados quando a página inicia
        public static readonly List<string> ExemploEmpresas = new List<string> {
            "Tesla", "Apple", "Samsung", "Americanas", "Embraer"
        };

        // Obtém o histórico da ibovespa de acordo com um intervalo e um range dado
        public static async Task<YahooModelIbovespa?> ObterHistorioIbovespa(string? intervalo, string? range)
        {
            // Caso nenhum parâmetro seja passado será usado o valor padrão
            intervalo ??= "30m";
            range ??= "2d";

            // Criar o URL personalizado com os parâmetros
            Uri uri = new Uri($"https://query1.finance.yahoo.com/v8/finance/chart/^BVSP?interval={intervalo}&range={range}");

            // Configurar o cliente de conexão
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
            );

            try
            {
                // Realizar a requisição à API
                HttpResponseMessage response = await client.GetAsync(uri);

                // Se der erro, retorna null
                if (!response.IsSuccessStatusCode) return null;

                // Converte os dados JSON vindos da API em um modelo C#
                YahooModelIbovespa? yahooModel = await response.Content.ReadFromJsonAsync<YahooModelIbovespa>();

                // Retorna o modelo
                return yahooModel;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return null;
        }

        // Pesquisar por empresas na API
        public static async Task<JObject?> Pesquisar(IMemoryCache cache, string query)
        {
            // Substitui os espaços por %20
            query = query.Replace(" ", "%20");

            // Cria o URL personalizado
            Uri uri = new Uri(@"https://query1.finance.yahoo.com/v1/finance/search?q=" + query);

            // Configura o cliente
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
            );

            try
            {
                HttpResponseMessage response;

                // Se a empresa pesquisada seja um empresa de exemplo, cria cache da resposta
                if (ExemploEmpresas.Contains(query))
                {
                    response = await cache.GetOrCreateAsync(query, async entry =>
                    {
                        // Define um tempo de 15 minutos
                        entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15);

                        // Faz a requisição a API
                        return await client.GetAsync(uri);
                    });
                } else
                {
                    // Faz a requisição a API
                    response = await client.GetAsync(uri);
                }

                // Se der errado, não faz nada
                if (!response.IsSuccessStatusCode) return null;

                // Neste caso, a resposta não é convertida em modelo c#, mas sim retornada como objeto JSON
                JObject objJson = JObject.Parse(await response.Content.ReadAsStringAsync());

                return objJson;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return null;
        }

        // Retorna a cotação de uma empresa de acordo com seu symbol, ex: TSLA
        public static async Task<List<string>?> ObterAcaoByTag(string tag)
        {
            // Remove espaços, caso existam
            tag = tag.Trim();

            // Cria a URL personalizada
            Uri uri = new Uri($"https://query1.finance.yahoo.com/v10/finance/quoteSummary/{tag}?modules=financialData");

            // Configura o cliente
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
            );

            try
            {
                // Faz a requisição a API
                HttpResponseMessage response = await client.GetAsync(uri);

                // Caso der erro, retorna null
                if (!response.IsSuccessStatusCode) return null;

                // Converte a resposta em JSON
                JObject objJson = JObject.Parse(await response.Content.ReadAsStringAsync());

                // Caso seja possível, retorna somente o valor da ação e a moeda
                JToken? sel = objJson?["quoteSummary"]?["result"]?[0]?["financialData"];
                JToken? selPrice = sel?["currentPrice"]?["raw"];
                JToken? selCur = sel?["financialCurrency"];

                // Se der erro, retorna null
                if (selPrice == null || selCur == null) return null;

                // Retorna a lista contendo a moeda e o valor da ação
                return new List<string> { selCur.ToString(), selPrice.ToString() };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return null;
        }
    }
}
