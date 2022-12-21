namespace Sistemas_Distribuidos.Models.Hg
{
    /*
     
        Modelo C# dos dados que vem da API como JSON.

        Os dados são convertidos de JSON para este modelo em C# que é identico ao formato do JSON

        Com isso, fica mais simples trabalhar com estes dados
     
     */

    // Usado para associar a tag (Nome da variável, ex: USD) a seu modelo instanciado
    public struct Item<T>
    {
        public string Tag;
        public T Model;

        public Item(string tag, T model)
        {
            Tag = tag;
            Model = model;
        }
    }

    public class Moeda
    {
        public string name { get; set; }
        public float buy { get; set; }
        public float? sell { get; set; }
        public float variation { get; set; }
    }

    public class Indice
    {
        public int? Id { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public float points { get; set; }
        public float variation { get; set; }
    }

    public class Corretora
    {
        public string name { get; set; }
        public List<string> format { get; set; }
        public float last { get; set; }
        public float? buy { get; set; }
        public float? sell { get; set; }
        public float variation { get; set; }
    }

    public class Taxa
    {
        public string date { get; set; }
        public float cdi { get; set; }
        public float selic { get; set; }
        public float daily_factor { get; set; }
        public float selic_daily { get; set; }
        public float cdi_daily { get; set; }
    }

    public class Currency
    {
        public string source { get; set; }
        public Moeda USD { get; set; }
        public Moeda EUR { get; set; }
        public Moeda GBP { get; set; }
        public Moeda ARS { get; set; }
        public Moeda CAD { get; set; }
        public Moeda AUD { get; set; }
        public Moeda JPY { get; set; }
        public Moeda CNY { get; set; }
        public Moeda BTC { get; set; }

        public List<Item<Moeda>> GetList()
        {
            return new List<Item<Moeda>>
            {
                new Item<Moeda>(nameof(USD), USD),
                new Item<Moeda>(nameof(EUR), EUR),
                new Item<Moeda>(nameof(GBP), GBP),
                new Item<Moeda>(nameof(AUD), AUD),
                new Item<Moeda>(nameof(JPY), JPY),
                new Item<Moeda>(nameof(CNY), CNY),
                new Item<Moeda>(nameof(BTC), BTC)
            };
        }
    }

    public class Stock
    {
        public Indice IBOVESPA { get; set; }
        public Indice IFIX { get; set; }
        public Indice NASDAQ { get; set; }
        public Indice DOWJONES { get; set; }
        public Indice CAC { get; set; }
        public Indice NIKKEI { get; set; }

        public List<Item<Indice>> GetList()
        {
            return new List<Item<Indice>>
            {
                new Item<Indice>(nameof(IBOVESPA), IBOVESPA),
                new Item<Indice>(nameof(IFIX), IFIX),
                new Item<Indice>(nameof(NASDAQ), NASDAQ),
                new Item<Indice>(nameof(DOWJONES), DOWJONES),
                new Item<Indice>(nameof(CAC), CAC),
                new Item<Indice>(nameof(NIKKEI), NIKKEI),
            };
        }
    }

    public class Bitcoin
    {
        public Corretora blockchain_info { get; set; }
        public Corretora coinbase { get; set; }
        public Corretora bitstamp { get; set; }
        public Corretora foxbit { get; set; }
        public Corretora mercadobitcoin { get; set; }

        public List<Item<Corretora>> GetList()
        {
            return new List<Item<Corretora>>
            {
                new Item<Corretora>(nameof(blockchain_info), blockchain_info),
                new Item<Corretora>(nameof(coinbase), coinbase),
                new Item<Corretora>(nameof(bitstamp), bitstamp),
                new Item<Corretora>(nameof(foxbit), foxbit),
                new Item<Corretora>(nameof(mercadobitcoin), mercadobitcoin),
            };
        }
    }

    public class Result
    {
        public Currency currencies { get; set; }
        public Stock stocks { get; set; }
        public List<string> available_sources { get; set; }
        public Bitcoin bitcoin { get; set; }
        public List<Taxa> taxes { get; set; }
    }

    public class HGModelBase
    {
        public string by { get; set; }
        public bool valid_key { get; set; }
        public Result results { get; set; }
        public float execution_time { get; set; }
        public bool from_cache { get; set; }
    }
}
