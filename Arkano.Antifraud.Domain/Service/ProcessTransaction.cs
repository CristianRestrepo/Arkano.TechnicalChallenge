using Arkano.Common.Common;
using Arkano.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arkano.Antifraud.Domain.Service
{
    public class ProcessTransaction : IProcessTransaction
    {
        private IAccumulated _accumulated;
        public ProcessTransaction(IAccumulated accumulated)
        {
            _accumulated = accumulated;
        }
        public void ValidateTransaction(ref TransactionEvent transactionEvent)
        {
            _accumulated.AddValueToAccumulated(transactionEvent.Value);
            if (transactionEvent.Value <= 2000 && _accumulated.GetAccumulated() <= 20000)
            {
                transactionEvent.IdState = (int)State.Approved;
            }
            else
            {
                transactionEvent.IdState = (int)State.Rejected;
            }
        }

        
    }
}
