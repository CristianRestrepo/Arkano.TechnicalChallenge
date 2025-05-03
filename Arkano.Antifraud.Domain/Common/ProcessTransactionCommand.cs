using Arkano.Common.Models;
using MediatR;

namespace Arkano.Antifraud.Domain.Common
{
    public record ProcessTransactionCommand : IRequest
    {
        public required TransactionEvent @Event { get; set; }
        public int Accumulated { get; set; }
    }
}
