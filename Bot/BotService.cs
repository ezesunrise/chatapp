using System.Globalization;
using System.Text.RegularExpressions;
using Bot.Queue;
using Bot.ValueObjects;
using CsvHelper;

namespace Bot;

public class BotService: IBotService
{
    private enum BotCommandEnum
    {
        STOCK
    }

    private readonly IMessageProducer _messagePublisher;
    public BotService(IMessageProducer producer)
    {
        _messagePublisher = producer;
    }

    public async Task ExcecuteCommand(string command)
    {
        var trimmedMsg = Regex.Replace(command, @"\s+", "");
        var data = trimmedMsg.Substring(1).Split("=");

        if (!Enum.GetNames<BotCommandEnum>().Contains(data[0].ToUpper()))
        {
            throw new ArgumentException($"'/{data[0]}' is an invalid command!");
        }
        var result = await GetQuote(data[1]);

        if (result.Date != "N/D")
        {
            var message = $"{result.Symbol} quote is ${result.Open} per share";
            _messagePublisher.SendMessage(message);
        }
    }

    private static async Task<StockQuote> GetQuote(string stockCode)
    {
        try
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri("https://stooq.com/q/l/");
            var response = await client.GetAsync($"?s={stockCode}&f=sd2t2ohlcv&h&e=csv");
            var stream = await response.Content.ReadAsStreamAsync();
            using var reader = new StreamReader(stream);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<StockQuote>().FirstOrDefault();
        }
        catch (Exception)
        {
            // log Exception
            return new StockQuote { Date = "N/D" };
        }
    }
}

