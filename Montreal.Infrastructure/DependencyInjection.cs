using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Montreal.Application.Common;
using Montreal.Infrastructure.Messaging;


namespace Montreal.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this
        IServiceCollection services
    )
    {
        services.AddSingleton<IMessagePublisher, RabbitMqPublisher>();
        return services;
    }
}