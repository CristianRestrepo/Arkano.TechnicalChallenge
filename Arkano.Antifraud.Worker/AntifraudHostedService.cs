using Arkano.Common.Consumer;
namespace Arkano.Worker;

public class AntifraudHostedService : IHostedService
{

    private readonly ILogger<AntifraudHostedService> _logger;
    private readonly IServiceProvider _serviceProvider; 
    const string _transactionsTopic = "Transactions";
  

    public AntifraudHostedService(ILogger<AntifraudHostedService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Consumer working..");
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                var eventConsumer = scope.ServiceProvider
                                    .GetRequiredService<IEventConsumer>();                  

                Task.Run(() => eventConsumer.Consume(_transactionsTopic), cancellationToken);
                
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in consumer");
            throw;
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Consumer stopped..");
        return Task.CompletedTask;
    }

    
}
