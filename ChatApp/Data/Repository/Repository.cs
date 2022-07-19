using System;
using ChatApp.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Data.Repository
{
    public class MessageRepository: IMessageRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public MessageRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Message Get(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Message>> GetAllAsync(int maxCount)
        {
            return await _dbContext.Messages
                .Include(x => x.Owner)
                .OrderByDescending(x => x.TimeStamp)
                .Take(maxCount)
                .Reverse()
                .ToListAsync();
        }
    }
}

