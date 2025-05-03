using Arkano.Common.Consumer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Arkano.Transaction.Infrastructure.Services;

public class TransactionHostedService : BackgroundService
{

    private readonly ILogger<TransactionHostedService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private const string _processedTopic = "Processed-Transactions";
    public TransactionHostedService(ILogger<TransactionHostedService> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;        
    }    

    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Consumer working..");
            using (IServiceScope scope = _serviceProvider.CreateScope())
            {
                var eventConsumer = scope.ServiceProvider
                                    .GetRequiredService<IEventConsumer>();

                Task.Run(() => eventConsumer.Consume(_processedTopic), cancellationToken);
            }            
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in consumer");
            throw;
        }
        return Task.CompletedTask;
    }

    
}
