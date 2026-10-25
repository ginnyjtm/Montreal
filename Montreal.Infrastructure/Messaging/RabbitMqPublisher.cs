using Montreal.Application.Common;
using RabbitMQ.Client;

namespace Montreal.Infrastructure.Massaging;

public class RabbitMqPuclisher: IMessagePublisher
{
    public async Task PublishAsync<T>(string queue, T message, CancellationToken ct = default)
    {
        
    }
}