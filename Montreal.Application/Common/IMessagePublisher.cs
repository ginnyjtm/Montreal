namespace Montreal.Application.Common
{
    public interface IMessagePublisher
    {
        Task PublishAsync<T>(string queue, T message, CancellationToken ct = default);
    }
}