using System;
using System.Text.RegularExpressions;
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
        private readonly IMessageRepository _repo;

        public ChatHub(IBotService bot, IMessageRepository repo)
        {
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
                await _bot.ExcecuteCommand(message);
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

        private async Task<Models.Message> SaveMessage(string message)
        {
            var newMessage = new Models.Message
            {
                OwnerId = Context.UserIdentifier,
                Content = message
            };
            await _repo.AddAsync(newMessage);
            return newMessage;
        }
    }
}

