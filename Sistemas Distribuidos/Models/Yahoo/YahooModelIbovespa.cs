namespace Sistemas_Distribuidos.Models.Yahoo
{
    /*
     
        Modelo C# dos dados que vem da API como JSON.

        Os dados são convertidos de JSON para este modelo em C# que é identico ao formato do JSON

        Com isso, fica mais simples trabalhar com estes dados
     
     */

    public class YahooModelIbovespa
    {
        public Chart chart { get; set; }
    }

    public class Chart
    {
        public Result[] result { get; set; }
        public object error { get; set; }
    }

    public class Result
    {
        public Meta meta { get; set; }
        public long[] timestamp { get; set; }
        public Indicators indicators { get; set; }
    }

    public class Meta
    {
        public string currency { get; set; }
        public string symbol { get; set; }
        public string exchangeName { get; set; }
        public string instrumentType { get; set; }
        public int firstTradeDate { get; set; }
        public int regularMarketTime { get; set; }
        public int gmtoffset { get; set; }
        public string timezone { get; set; }
        public string exchangeTimezoneName { get; set; }
        public float regularMarketPrice { get; set; }
        public float chartPreviousClose { get; set; }
        public float previousClose { get; set; }
        public int scale { get; set; }
        public int priceHint { get; set; }
        public Currenttradingperiod currentTradingPeriod { get; set; }
        public Tradingperiod[][] tradingPeriods { get; set; }
        public string dataGranularity { get; set; }
        public string range { get; set; }
        public string[] validRanges { get; set; }
    }

    public class Currenttradingperiod
    {
        public Pre pre { get; set; }
        public Regular regular { get; set; }
        public Post post { get; set; }
    }

    public class Pre
    {
        public string timezone { get; set; }
        public int end { get; set; }
        public int start { get; set; }
        public int gmtoffset { get; set; }
    }

    public class Regular
    {
        public string timezone { get; set; }
        public int end { get; set; }
        public int start { get; set; }
        public int gmtoffset { get; set; }
    }

    public class Post
    {
        public string timezone { get; set; }
        public int end { get; set; }
        public int start { get; set; }
        public int gmtoffset { get; set; }
    }

    public class Tradingperiod
    {
        public string timezone { get; set; }
        public int end { get; set; }
        public int start { get; set; }
        public int gmtoffset { get; set; }
    }

    public class Indicators
    {
        public Quote[] quote { get; set; }
    }

    public class Quote
    {
        public float?[] close { get; set; }
        public float?[] high { get; set; }
        public long?[] volume { get; set; }
        public float?[] low { get; set; }
        public float?[] open { get; set; }
    }
}