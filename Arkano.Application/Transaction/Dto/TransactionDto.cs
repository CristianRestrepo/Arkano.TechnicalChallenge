using Arkano.Common.Common;

namespace Arkano.Transaction.Application.Transaction.Dto
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public Guid SourceAccountId { get; set; }
        public Guid TargetAccountId { get; set; }
        public int TransferTypeId { get; set; }
        public State IdState { get; set; }
        public int Value { get; set; }
        public DateTime CreatedAd { get; set; }
    }
}
