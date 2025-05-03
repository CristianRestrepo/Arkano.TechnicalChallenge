
using Arkano.Common.Models;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
namespace Arkano.Common.Producer
{

    public class EventProducer : IEventProducer
    {
        private readonly KafkaSettings _KafkaSettings;
        private readonly ILogger<EventProducer> _logger;

        public EventProducer(IOptions<KafkaSettings> kafkaSettings, ILogger<EventProducer> logger)
        {
            _KafkaSettings = kafkaSettings.Value;
            _logger = logger;
        }

        public async Task SendAsync<T>(string topic, T @event)
        {
            try
            {
                var config = new ProducerConfig
                {
                    BootstrapServers = $"{_KafkaSettings.Hostname}:{_KafkaSettings.Port}",
                    AllowAutoCreateTopics = true
                };

                using var producer = new ProducerBuilder<string, string>(config)
                .SetKeySerializer(Serializers.Utf8)
                .SetValueSerializer(Serializers.Utf8)
                .Build();

                var eventMessage = new Message<string, string>
                {
                    Key = Guid.NewGuid().ToString(),
                    Value = JsonSerializer.Serialize(@event)
                };  

                var deliveryStatus = await producer.ProduceAsync(topic, eventMessage);

                if (deliveryStatus.Status == PersistenceStatus.NotPersisted)
                {
                    throw new Exception(@$"
                         No se pudo enviar el mensaje {@event!.GetType().Name} 
                         hacia el topic - {topic}, 
                         por la siguiente razon: {deliveryStatus.Message}");
                }
                _logger.LogInformation("Event sent!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending event");
                throw;
            }           
        }        
    }
}
