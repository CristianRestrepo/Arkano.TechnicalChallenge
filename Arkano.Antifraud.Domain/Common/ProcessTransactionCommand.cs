using Arkano.Common.Models;
using Arkano.Common.Producer;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arkano.Antifraud.Domain.Common
{
    public record ProcessTransactionCommand : IRequest
    {
        public required TransactionEvent @Event { get; set; }
        public int Accumulated { get; set; }
    }
}
