﻿using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Bot.Queue
{
    public class RabbitMQMessageProducer: IMessageProducer
    {
        public RabbitMQMessageProducer()
        {
        }

        public void SendMessage<T>(T message)
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            //channel.QueueDeclarePassive("stockQuotes");

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "", routingKey: "stockQuotes", body: body);
        }
    }
}

