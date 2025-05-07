using Arkano.Common.Models;

namespace Arkano.Antifraud.Domain.Service
{
    public interface IProcessTransaction
    {
        void ValidateTransaction(ref TransactionEvent transactionEvent);
    }
}
