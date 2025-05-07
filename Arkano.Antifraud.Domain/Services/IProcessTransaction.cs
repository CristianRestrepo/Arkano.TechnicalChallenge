using Arkano.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arkano.Antifraud.Domain.Services
{
    public interface IProcessTransaction
    {
        void ValidateTransaction(ref TransactionEvent transactionEvent);
    }
}
