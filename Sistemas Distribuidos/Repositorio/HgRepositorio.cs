using Sistemas_Distribuidos.Data;
using Sistemas_Distribuidos.Models.Hg;

namespace Sistemas_Distribuidos.Repositorio
{
    public class HgRepositorio : IHgRepositorio
    {
        private BancoContext _bancoContext;

        // Injetando dependencias
        public HgRepositorio(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        // Adicionar todos os dados que retornam da API no banco de dados
        // Esta inclusão ocorre uma vez por dia
        public HGModelBase Adicionar(HGModelBase data)
        {
            MoedaModel moedaDB;
            IndiceModel indiceDB;
            CorretoraModel corretoraDB;
            TaxaModel taxaDB;

            // Salva todas as moedas vindas da API no Banco de dados
            foreach (Item<Moeda> item in data.results.currencies.GetList())
            {
                moedaDB = new MoedaModel()
                {
                    Name = item.Model.name,
                    Tag = item.Tag,
                    Buy = item.Model.buy,
                    Sell = item.Model.sell,
                    Variation = item.Model.variation,
                    UpdateAt = DateTime.Now,
                };

                // Salvar
                _bancoContext.Moedas.Add(moedaDB);
            }

            // Salva todos os indices vindos da API no Banco de dados
            foreach (Item<Indice> item in data.results.stocks.GetList())
            {
                indiceDB = new IndiceModel()
                {
                    Name = item.Model.name,
                    Tag = item.Tag,
                    Location = item.Model.location,
                    Points = item.Model.points,
                    Variation = item.Model.variation,
                    UpdateAt = DateTime.Now,
                };

                // Salvar
                _bancoContext.Indices.Add(indiceDB);
            }

            // Salva todas as corretoras vindas da API no Banco de dados
            foreach (Item<Corretora> item in data.results.bitcoin.GetList())
            {
                corretoraDB = new CorretoraModel()
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
                };

                // Salvar
                _bancoContext.Corretoras.Add(corretoraDB);
            }

            // Caso possua as taxas de juros brasileiras, incluir também
            if (data.results.taxes.Count > 0) {
                taxaDB = new TaxaModel()
                {
                    Date = DateTime.Parse(data.results.taxes[0].date),
                    Cdi = data.results.taxes[0].cdi,
                    Selic = data.results.taxes[0].selic,
                    DailyFactor = data.results.taxes[0].daily_factor,
                    SelicDaily = data.results.taxes[0].selic_daily,
                    CdiDaily = data.results.taxes[0].cdi_daily,
                    UpdateAt = DateTime.Now,
                };

                // Salvar
                _bancoContext.Taxas.Add(taxaDB);
            }

            // Salvar todas as alterações no banco
            _bancoContext.SaveChanges();

            return data;
        }

        // Obter o histórico da moeda quando informado sua tag, ex: USD, EUR
        public List<MoedaModel>? ObterHistoricoMoeda(string tag)
        {
            // Realizar a query no banco de dados
            return _bancoContext.Moedas
                .Where(e => e.Tag == tag)?
                .OrderBy(e => e.UpdateAt)
                .ToList();
        }

        // Usado somente para Debug:
        public void UpdatePersonalizado()
        {
            var resp = _bancoContext.Moedas.Where(x => x.Id >= 113 && x.Id <= 119).ToList();

            if (resp.Count == 0) { Console.WriteLine("Deu ERROOOOO"); return; }

            Console.WriteLine("Inicio");
            foreach (var item in resp)
            {

                item.UpdateAt = DateTime.Parse("18/12/2022 13:43:12");

                _bancoContext.Moedas.Update(item);


                Console.WriteLine(item.Name);
            }

            _bancoContext.SaveChanges();
            Console.WriteLine("Fim");
        }
    }
}
