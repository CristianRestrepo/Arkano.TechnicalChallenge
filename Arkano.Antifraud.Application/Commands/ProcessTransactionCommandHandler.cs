using Arkano.Antifraud.Domain.Services;
using Arkano.Common.Models;
using Arkano.Common.Producer;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Arkano.Antifraud.Application.Commands
{
    public record ProcessTransactionCommand : IRequest
    {
        public required TransactionEvent @Event { get; set; }     
    }

    public class ProcessTransactionCommandHandler : IRequestHandler<ProcessTransactionCommand>
    {
        private readonly IEventProducer _producer;
        private readonly ILogger<ProcessTransactionCommandHandler> _logger;
        private const string _processedTopic = "Processed-Transactions";
        private readonly IProcessTransaction _processTransaction;

        public ProcessTransactionCommandHandler(IEventProducer producer, ILogger<ProcessTransactionCommandHandler> logger, IProcessTransaction processTransaction)
        {
            _producer = producer;
            _logger = logger;
            _processTransaction = processTransaction;
        }

        public async Task Handle(ProcessTransactionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Processing transaction: {request.@Event.Id}");
                TransactionEvent transactionEvent = request.@Event;
                _processTransaction.ValidateTransaction(ref transactionEvent);              
                await _producer.SendAsync<TransactionEvent>(_processedTopic, transactionEvent);
            }
            catch (Exception ex)
            {
                _logger.LogError (ex, "Error processing transaction: {Id}" , request.Event.Id);
                throw;
            }          
        }      
    }
}
