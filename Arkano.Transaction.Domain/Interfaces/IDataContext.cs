using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arkano.Transaction.Domain.Interfaces
{

    public interface IDataContext
    {
        public DbSet<Entities.Transaction> Transactions { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

    
   
}
