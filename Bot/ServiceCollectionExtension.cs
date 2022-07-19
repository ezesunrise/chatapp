using System;
using Bot.Queue;
using Microsoft.Extensions.DependencyInjection;

namespace Bot
{
    public static class ServiceCollectionExtension
    {
        public static void AddBotServices(this IServiceCollection services)
        {
            services.AddScoped<IBotService, BotService>();
            services.AddScoped<IMessageProducer, RabbitMQMessageProducer>();
        }
    }
}

