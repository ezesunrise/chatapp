using System;
namespace ChatApp.Hubs
{
    public interface IChatHub
    {
        public Task JoinedMessage(string message);
        public Task ReceiveMessage(Pages.MessageModel message);

    }
}

