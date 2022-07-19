using System.Dynamic;
using Bot;
using ChatApp.Data.Repository;
using ChatApp.Hubs;
using ChatApp.Models;
using Microsoft.AspNetCore.SignalR;
using Moq;
namespace ChatApp.Test;

public class ChatTests
{
    private readonly Mock<IBotService> _bot;
    private readonly Mock<IMessageRepository> _repo;

    public ChatTests()
    {
        _bot = new Mock<IBotService>();
        _repo = new Mock<IMessageRepository>();

        var messgs = new List<Message>
        {
            new Message{ Content = "1 Message", OwnerId = "" },
            new Message{ Content = "2 Message", OwnerId = "" },
            new Message{ Content = "3 Message", OwnerId = "" },
            new Message{ Content = "4 Message", OwnerId = "" }
        };
        _repo.Setup(ent => ent.GetAllAsync(50)).Returns(Task.FromResult(messgs.AsEnumerable()));
    }

    [Theory]
    [InlineData("Lovely")]
    [InlineData("That is a great play!")]
    [InlineData("Is the expression '5 > 11' true?")]
    public async Task ChatHub_ShouldCallAllClients_WhenAClientSendsMessageWhichIsNotAcommand(string message)
    {
        // arrange
        var mockClients = new Mock<IHubCallerClients<IChatHub>>();
        Mock<IClientProxy> mockClientProxy = new Mock<IClientProxy>();

        _repo.Setup(ent => ent.AddAsync(new Message { Content = message, OwnerId = Guid.NewGuid().ToString() }));
        mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object as IChatHub);

        var chatHub = new ChatHub(_bot.Object, _repo.Object)
        {
            Clients = mockClients.Object
        };

        // act
        await chatHub.SendMessage(message);


        // assert
        //mockClients.Verify(clients => clients.All, Times.Once);

    }

    [Fact]
    public async Task SendMessage_ShouldNotCallClients_WhenAClientSendsACommand()
    {
        var message = "/stock=appl.us";
        // arrange
        var mockClients = new Mock<IHubCallerClients<IChatHub>>();
        Mock<IClientProxy> mockClientProxy = new Mock<IClientProxy>();

        _repo.Setup(ent => ent.AddAsync(new Message { Content = message, OwnerId = Guid.NewGuid().ToString() }));
        mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object as IChatHub);

        var chatHub = new ChatHub(_bot.Object, _repo.Object)
        {
            Clients = mockClients.Object
        };

        // act
        await chatHub.SendMessage(message);


        // assert
        //mockClients.Verify(clients => clients.All, Times.Never);

    }

    [Theory]
    [InlineData("/stock=appl.us")]
    public async Task SendMessage_ShouldCallBot_WhenMessageStartsWithSlash(string message)
    {
        //_bot.Setup(x => x.ExcecuteCommand(message)).Returns();
        //Assert.True(message == "");
    }

}
