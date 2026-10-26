using Montreal.Application.Common;
using RabbitMQ.Client;

namespace Montreal.Infrastructure.Messaging;

public class RabbitMqPublisher : IMessagePublisher
{
    public async Task PublishAsync<T>(string queue, T message, CancellationToken ct = default)
    {
        Console.WriteLine("Log:" + queue + " - " + message);
    }
}