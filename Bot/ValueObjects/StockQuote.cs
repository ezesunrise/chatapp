using System;
namespace Bot.ValueObjects
{
    public class StockQuote
    {
        public StockQuote()
        {
        }

        public string Symbol { get; set; }

        public string Date { get; set; }
        public string Open { get; set; }
    }
}

