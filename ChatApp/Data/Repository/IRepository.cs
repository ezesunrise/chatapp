using System;
using ChatApp.Models;

namespace ChatApp.Data.Repository
{
    public interface IMessageRepository
    {
        Task<IEnumerable<Message>> GetAllAsync(int maxCount);
        Message Get(string id);
        Task AddAsync(Message newMessage);
    }
}

