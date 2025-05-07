using Arkano.Common.Common;
using Arkano.Common.Models;
using Arkano.Common.Producer;
using Arkano.Transaction.Application.Transaction.Dto;
using Arkano.Transaction.Domain.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Arkano.Transaction.Application.Transaction.Commands
{
    public record CreateTransactionCommand : IRequest<TransactionDto> {
        public Guid SourceAccountId { get; set; } 
        public Guid TargetAccountId { get; set; } 
        public int TransferTypeId { get; set; }    
        public int Value { get; set; }
    }

    public class CreateTransactionCommandHandler : IRequestHandler<CreateTransactionCommand, TransactionDto>
    {
        private readonly IDataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly IEventProducer _producer;
        private readonly ILogger<CreateTransactionCommandHandler> _logger;
        public CreateTransactionCommandHandler(IDataContext dataContext, IMapper mapper, IEventProducer producer, ILogger<CreateTransactionCommandHandler> logger)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _producer = producer;
            _logger = logger;
        }

        public async Task<TransactionDto> Handle(CreateTransactionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var transaction = new Domain.Entities.Transaction
                {
                    SourceAccountId = request.SourceAccountId,
                    TargetAccountId = request.TargetAccountId,
                    TransferTypeId = request.TransferTypeId,
                    IdState = (int)State.Pending,
                    CreatedAd = DateTime.UtcNow,
                    Value = request.Value
                };
                _dataContext.Transactions.Add(transaction);
                await _dataContext.SaveChangesAsync(cancellationToken);

                await _producer.SendAsync<TransactionEvent>("Transactions", _mapper.Map<TransactionEvent>(transaction));

                return _mapper.Map<TransactionDto>(transaction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating transaction");
                throw;
            }
        }
    }
}
