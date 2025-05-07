
using System.Text.Json;
using Arkano.Common.Models;
using Arkano.Transaction.Application.Transaction.Commands;
using Confluent.Kafka;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Arkano.Common.Consumer
{
    public class EventConsumer : IEventConsumer
    {
        private readonly ILogger<EventConsumer> _logger;
        private readonly ConsumerConfig _config;
        private readonly IServiceScopeFactory _serviceProvider;

        public EventConsumer(
            IOptions<ConsumerConfig> config,
            IServiceScopeFactory serviceProvider,
            ILogger<EventConsumer> logger)
        {
            _config = config.Value;
            _serviceProvider = serviceProvider;
            _logger = logger;
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

                        IServiceScope scope = _serviceProvider.CreateScope();
                        var mediator = scope.ServiceProvider
                                    .GetRequiredService<IMediator>();

                        await ProcessTransaction(@event, mediator);

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

        private static async Task ProcessTransaction(TransactionEvent @event, IMediator mediator)
        {
            if (@event != null)
            {
                var command = new UpdateTransactionCommand
                {
                    Id = @event.Id,
                    IdState = @event.IdState,
                };
                await mediator.Send(command);
            }
        }
    }
}
