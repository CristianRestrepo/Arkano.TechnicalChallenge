using Arkano.Transaction.Domain.Common;
using Arkano.Transaction.Domain.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arkano.Transaction.Application.Transaction.Commands
{   

    public class UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand, Unit>
    {
        private readonly IDataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateTransactionCommandHandler> _logger;

        public UpdateTransactionCommandHandler(IDataContext dataContext, IMapper mapper, ILogger<UpdateTransactionCommandHandler> logger)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Unit> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var transaction = await _dataContext.Transactions.Where(t => t.Id.ToString().Equals(request.Id.ToString())).FirstOrDefaultAsync(cancellationToken);

                if (transaction != null)
                {
                    transaction.IdState = request.IdState;
                    _dataContext.Transactions.Update(transaction);
                    await _dataContext.SaveChangesAsync(cancellationToken);
                }
                else {
                    throw new Exception("Transaction not found");
                }                    
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating transaction");
                throw;
            }
            return new();
        }
    }
}
