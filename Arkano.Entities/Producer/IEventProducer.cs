

namespace Arkano.Common.Producer
{
    public interface IEventProducer
    {
        Task SendAsync<T>(string topic, T transactionEvent);
    }
}

