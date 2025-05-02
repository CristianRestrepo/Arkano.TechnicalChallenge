using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arkano.Transaction.Domain.Common
{
    public record UpdateTransactionCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public int IdState { get; set; }
    }
}
