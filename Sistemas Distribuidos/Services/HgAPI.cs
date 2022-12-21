using Microsoft.Extensions.Caching.Memory;
using Sistemas_Distribuidos.Data;
using Sistemas_Distribuidos.Models.Hg;

namespace Sistemas_Distribuidos.Services
{
    // Responsável por fazer as requisições para a API
    public class HgAPI
    {
        // Configurar a única rota acessível por esta API

        static readonly string hg_key = Environment.GetEnvironmentVariable("HQ_KEY") ?? throw new Exception("Variável de ambiente HQ_KEY não encontrada");

        static readonly Uri uri = new Uri($"https://api.hgbrasil.com/finance?key={hg_key}");
        // Configurar o cliente
        static readonly HttpClient client = new HttpClient();

        // Criar um cache de resposta manualmente
        static HGModelBase? respLast = null;

        // Obter todos os detalhes financeiros disponíveis na HG Finance
        public static async Task<HGModelBase?> GetFinancialDetails()
        {
            // Configurar o cliente que fará a requisição
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
            );

            try
            {
                // Faz a requisição
                HttpResponseMessage response = await client.GetAsync(uri);

                // Caso der erro, retorna null
                if (!response.IsSuccessStatusCode) return null;

                // Caso der certo, converte o JSON para o modelo C# e retorna o resultado
                respLast = await response.Content.ReadFromJsonAsync<HGModelBase>();

                return respLast;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return null;
        }

        // Faz o mesmo que a função anterior, mas colocando um cache de 5 minutos
        public static async Task<HGModelBase?> GetFinancialDetails(IMemoryCache cache)
        {
            // Cria o cache
            HGModelBase? result = await cache.GetOrCreateAsync("financialDetails", async (entry) =>
            {
                // Define 5 minutos de cache
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

                // Retorna o resultado da função anterior
                return await GetFinancialDetails();
            });

            // Se der erro, remove o cache para não ficar dando erro por 5 minutos, pois o null entra no cache
            if (result == null)
            {
                cache.Remove("financialDetails");
            }

            return result;
        }

        // Obter cotação das moedas de acordo com o cache das requisições das funções anteriores
        public static async Task<List<MoedaModel>?> ObterCotacaoMoedas(IMemoryCache cache)
        {
            // Obter os dados
            HGModelBase? hgbase = await GetFinancialDetails(cache);

            // Se der erro, retorna null
            if (hgbase == null) return null;

            List<MoedaModel> result = new List<MoedaModel>();

            // Preenche a lista de MoedaModel acima com os dados vindos da API
            foreach (Item<Moeda> item in hgbase.results.currencies.GetList())
            {
                result.Add(
                    new MoedaModel()
                    {
                        Name = item.Model.name,
                        Tag = item.Tag,
                        Buy = item.Model.buy,
                        Sell = item.Model.sell,
                        Variation = item.Model.variation,
                        UpdateAt = DateTime.Now,
                    }
                );
            }

            // Retorna os dados das moedas
            return result;
        }

        // Obter cotação das corretoras de bitcoin de acordo com o cache das requisições das funções anteriores
        public static async Task<List<CorretoraModel>?> ObterCotacaoCorretoras(IMemoryCache cache)
        {
            // Obter os dados
            HGModelBase? hgbase = await GetFinancialDetails(cache);

            // Se der erro, retorna null
            if (hgbase == null) return null;

            List<CorretoraModel> result = new List<CorretoraModel>();

            // Preenche a lista de CorretoraModel acima com os dados vindos da API
            foreach (Item<Corretora> item in hgbase.results.bitcoin.GetList())
            {
                result.Add(
                    new CorretoraModel()
                    {
                        Name = item.Model.name,
                        Tag = item.Tag,
                        FormatMoeda = item.Model.format[0],
                        FormatIdioma = item.Model.format[1],
                        Last = item.Model.last,
                        Buy = item.Model.buy,
                        Sell = item.Model.sell,
                        Variation = item.Model.variation,
                        UpdateAt = DateTime.Now,
                    }
                );
            }

            // Retorna o resultado da cotação de bitcoin das corretoras
            return result;
        }

        // As duas funções abaixo tem a função de converter Dolar para Real de acordo com
        // a requisição mais recente da cotação das moedas

        // Converte um dado float em Dolar para Real
        public static float? ConverterDolarPraReal(float dolar)
        {
            // Se não tiver sido feita nenhuma requisição, faz a requisição
            if (respLast == null) GetFinancialDetails().Wait();
            // Se der erro, retorna null
            if (respLast == null || respLast.results.currencies.USD.buy == 0) return null;

            // Retorna o resultado
            return dolar * respLast.results.currencies.USD.buy;
        }

        // Converte uma dada string representando um valor em Dolar para Real
        public static float? ConverterDolarPraReal(string dolar)
        {
            // Se não tiver sido feita nenhuma requisição, faz a requisição
            if (respLast == null) GetFinancialDetails().Wait();
            // Se der erro, retorna null
            if (respLast == null || respLast.results.currencies.USD.buy == 0) return null;

            // Retorna o resultado
            return float.Parse(dolar) * respLast.results.currencies.USD.buy;
        }
    }
}
