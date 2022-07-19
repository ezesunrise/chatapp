using System;
using Bot;
using ChatApp.Data;
using ChatApp.Data.Repository;
using ChatApp.Pages;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.Hubs
{
    public class ChatHub: Hub<IChatHub>
    {
        private readonly IBotService _bot;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMessageRepository _repo;

        public ChatHub(IBotService bot, ApplicationDbContext dbContext, IMessageRepository repo)
        {
            _dbContext = dbContext;
            _repo = repo;
            _bot = bot;
        }

        public async Task JoinRoom(string roomName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        public async Task LeaveRoom(string roomName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }

        public async Task SendJoinedMessage()
        {
            await Clients.Others.JoinedMessage(Context.User?.Identity?.Name);
        }

        public async Task SendMessage(string message)
        {
            if (message.StartsWith("/") && message.Length > 1)
            {
                await ExecuteBotCommand(message);
            }
            else
            {
                var savedMessage = await SaveMessage(message);

                var messageObj = new MessageModel
                {
                    Id = savedMessage.Id,
                    Content = savedMessage.Content,
                    TimeStamp = savedMessage.TimeStamp,
                    Owner = Context.User?.Identity?.Name
                };
                await Clients.All.ReceiveMessage(messageObj);
            }
        }

        private async Task ExecuteBotCommand(string message)
        {
            var data = message.Substring(1).Split("=");

            if (data[0] != "stock")
            {
                throw new Exception($"'/{data[0]}' is an invalid command!");
            }

            await _bot.GetStockQuote(data[1]);
        }

        private async Task<Models.Message> SaveMessage(string message)
        {
            var newMessage = new Models.Message
            {
                OwnerId = Context.UserIdentifier,
                Content = message
            };
            await _dbContext.Messages.AddAsync(newMessage);
            await _dbContext.SaveChangesAsync();
            return newMessage;
        }
    }
}

