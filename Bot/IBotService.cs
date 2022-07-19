using System;
namespace Bot
{
    public interface IBotService
    {
        Task ExcecuteCommand(string command);
    }
}

