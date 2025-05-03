using Arkano.Transaction.Application.Transaction.Dto;
using Arkano.Transaction.Domain.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Arkano.Transaction.Application.Transaction.Querys
{
    public record GetTransactionQuery() : IRequest<List<TransactionDto?>>
    {
        public Guid? TransactionExternalId { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

    class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionQuery, List<TransactionDto?>>
    {
        private readonly IDataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly ILogger<GetTransactionByIdQueryHandler> _logger;
        public GetTransactionByIdQueryHandler(IDataContext dataContext, IMapper mapper, ILogger<GetTransactionByIdQueryHandler> logger)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<TransactionDto?>> Handle(GetTransactionQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var transactions = await _dataContext.Transactions
                .Where(x => (request.TransactionExternalId != null && request.CreatedAt != null 
                     && x.Id == request.TransactionExternalId && request.CreatedAt.Equals(x.CreatedAd)) ||
                     ( 
                        (request.CreatedAt == null && x.Id == request.TransactionExternalId) ||
                        (request.TransactionExternalId == null && request.CreatedAt.Equals(x.CreatedAd)) ||
                        (request.TransactionExternalId == null && request.CreatedAt == null)
                     ))
                .ToListAsync(cancellationToken);
                if (transactions.Any())
                {
                    var transactionsDto = _mapper.Map<List<TransactionDto>>(transactions);
                    return transactionsDto!;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error getting transaction by id");
                throw;
            }
            return new();
        }
    }
}
