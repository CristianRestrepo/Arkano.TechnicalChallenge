namespace Arkano.Common.Consumer
{
    public interface IEventConsumer
    {
        Task Consume(string topic);
    }
}