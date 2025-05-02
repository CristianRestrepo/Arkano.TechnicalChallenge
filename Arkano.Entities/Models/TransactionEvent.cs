using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arkano.Common.Models
{
    public class TransactionEvent
    {
        public Guid Id { get; set; }      
        public int Value { get; set; }
        public int IdState { get; set; }
    }
}
