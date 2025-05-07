using Arkano.Common.Common;
using Arkano.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arkano.Antifraud.Domain.Services
{
    public class ProcessTransaction : IProcessTransaction
    {
        public void ValidateTransaction(ref TransactionEvent transactionEvent)
        {
            AddValueToAccumulated(transactionEvent.Value);
            if (transactionEvent.Value <= 2000 && Accumulated._accumulated <= 20000)
            {
                transactionEvent.IdState = (int)State.Approved;
            }
            else
            {
                transactionEvent.IdState = (int)State.Rejected;
            }
        }

        private void AddValueToAccumulated(int value)
        {
            if (Accumulated._accumulatedDate.Date.Equals(DateTime.Today.Date))
            {
                Accumulated._accumulated += value;
            }
            else
            {
                Accumulated._accumulatedDate = DateTime.Today;
                Accumulated._accumulated = value;
            }
        }
    }
}
