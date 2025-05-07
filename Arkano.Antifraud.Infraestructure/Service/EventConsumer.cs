using Arkano.Antifraud.Application.Commands;
using Arkano.Common.Consumer;
using Arkano.Common.Models;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;


namespace Arkano.Antifraud.Infrastructure.Service
{
    public class EventConsumer : IEventConsumer
    {
        private readonly ILogger<EventConsumer> _logger;
        private readonly ConsumerConfig _config;
        private readonly IServiceScopeFactory _scopeFactory;

        public EventConsumer(
            IOptions<ConsumerConfig> config,
            ILogger<EventConsumer> logger,
            IServiceScopeFactory scopeFactory
          )
        {
            _config = config.Value;
            _logger = logger;
            _scopeFactory = scopeFactory;

        }

        public async Task Consume(string topic)
        {
            try
            {
                using var consumer = new ConsumerBuilder<string, string>(_config)
                       .SetKeyDeserializer(Deserializers.Utf8)
                       .SetValueDeserializer(Deserializers.Utf8)
                       .Build();

                consumer.Subscribe(topic);

                while (true)
                {
                    var consumeResult = consumer.Consume();
                    if (consumeResult is null) continue;
                    if (consumeResult.Message is null) continue;

                    var options = new JsonSerializerOptions { };
                    var @event = JsonSerializer
                                    .Deserialize<TransactionEvent>(
                                        consumeResult.Message.Value,
                                        options
                                    );

                    if (@event is null)
                    {
                        throw new ArgumentNullException("Message could not be processed");
                    }
                                       
                    using var scope = _scopeFactory.CreateScope();
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    await mediator.Send(new ProcessTransactionCommand()
                    {
                        Event = @event,
                    });

                    _logger.LogInformation($"Message received {consumeResult.Message.Value}");
                    consumer.Commit(consumeResult);
                }

            }
            catch (ConsumeException ex)
            {
                _logger.LogError(ex, "Error consuming message");
                throw;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error deserializing message");
                throw;
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Message could not be processed");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing Message");
                throw;
            }
        }        
    }
}