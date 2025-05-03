using Microsoft.EntityFrameworkCore;

namespace Arkano.Transaction.Domain.Interfaces
{

    public interface IDataContext
    {
        public DbSet<Entities.Transaction> Transactions { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

    
   
}
