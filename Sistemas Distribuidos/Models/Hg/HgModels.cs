namespace Sistemas_Distribuidos.Models.Hg
{
    /*
     
        Modelo C# dos dados que vem e vão para o banco de dados SQL.

        Os dados vindos da API são convertidos para este modelo antes de salvar no banco de dados
     
     */

    public class MoedaModel
    {
        public int Id { get; set; }
        public string Tag { get; set; }
        public string Name { get; set; }
        public float Buy { get; set; }
        public float? Sell { get; set; }
        public float Variation { get; set; }
        public DateTime UpdateAt { get; set; }

        public static List<string> MoedasAceitas()
        {
            return new List<string>
            {
                "USD", "EUR", "GBP", "AUD", "JPY", "CNY", "BTC"
            };
        }
    }

    public class IndiceModel
    {
        public int Id { get; set; }
        public string Tag { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public float Points { get; set; }
        public float Variation { get; set; }
        public DateTime UpdateAt { get; set; }
    }

    public class CorretoraModel
    {
        public int Id { get; set; }
        public string Tag { get; set; }
        public string Name { get; set; }
        public string FormatMoeda { get; set; }
        public string FormatIdioma { get; set; }
        public float Last { get; set; }
        public float? Buy { get; set; }
        public float? Sell { get; set; }
        public float Variation { get; set; }
        public DateTime UpdateAt { get; set; }
    }

    public class TaxaModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public float Cdi { get; set; }
        public float Selic { get; set; }
        public float DailyFactor { get; set; }
        public float SelicDaily { get; set; }
        public float CdiDaily { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}