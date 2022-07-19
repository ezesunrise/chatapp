using Bot;
using Bot.Queue;
using Microsoft.AspNetCore.SignalR;
using Moq;
namespace ChatApp.Test;

public class BotServiceTests
{
    private readonly Mock<IMessageProducer> _producer;

    public BotServiceTests()
    {
        _producer = new Mock<IMessageProducer>();

    }

    [Theory]
    [InlineData("/stock=aapl.us")]
    public async Task SendMessage_CompletesSuccessfuly_WhenCommandIsValid(string command)
    {
        var bot = new BotService(_producer.Object);
        await bot.ExcecuteCommand(command);
        Assert.True(true);
    }

    [Theory]
    [InlineData("/stock=appppl.us")]
    public async Task SendMessage_Completes_WhenStockCodeIsNotFound(string command)
    {
        var bot = new BotService(_producer.Object);
        await bot.ExcecuteCommand(command);
        Assert.True(true);
    }

    [Theory]
    [InlineData("/quote=aapl.us")]
    public async Task SendMessage_ThrowsError_WhenCommandIsNotValid(string command)
    {
        var bot = new BotService(_producer.Object);
        await Assert.ThrowsAsync<ArgumentException>(async () => await bot.ExcecuteCommand(command));
    }
}
