using MediatR;

namespace Arkano.Transaction.Domain.Common
{
    public record UpdateTransactionCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public int IdState { get; set; }
    }
}
