using Arkano.Antifraud.Domain.Common;
using Arkano.Common.Common;
using Arkano.Common.Models;
using Arkano.Common.Producer;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Arkano.Antifraud.Application.Commands
{
    
    class ProcessTransactionCommandHandler : IRequestHandler<ProcessTransactionCommand>
    {
        private readonly IEventProducer _producer;
        private readonly ILogger<ProcessTransactionCommandHandler> _logger;
        private const string _processedTopic = "Processed-Transactions";
        public ProcessTransactionCommandHandler(IEventProducer producer, ILogger<ProcessTransactionCommandHandler> logger)
        {
            _producer = producer;
            _logger = logger;
        }

        public async Task Handle(ProcessTransactionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation($"Processing transaction: {request.Event.Id}");

                TransactionEvent transactionEvent = new TransactionEvent
                {
                    Id = request.Event.Id,
                    Value = request.Event.Value
                };

                if (request.Event.Value <= 2000 && request.Accumulated <= 20000)
                {
                    transactionEvent.IdState = (int)State.Approved;
                }
                else
                {
                    transactionEvent.IdState = (int)State.Rejected;
                }
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
