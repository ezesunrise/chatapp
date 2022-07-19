using System;
namespace Bot.Queue
{
    public interface IMessageProducer
    {
        void SendMessage<T>(T message);
    }
}

