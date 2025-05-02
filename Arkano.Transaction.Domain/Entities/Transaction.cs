using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arkano.Transaction.Domain.Entities
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public Guid SourceAccountId { get; set; }
        public Guid TargetAccountId { get; set; }
        public int TransferTypeId { get; set; }
        public int IdState { get; set; }
        public int Value { get; set; }
        public DateTime CreatedAd { get; set; }
    }
}
