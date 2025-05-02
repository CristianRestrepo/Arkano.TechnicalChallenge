using Arkano.Transaction.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace Arkano.Transaction.Infrastructure
{
    public partial class DataContext : DbContext, IDataContext
    {
       
       
        public DataContext()
        {                
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Arkano.Transaction.Domain.Entities.Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Arkano.Transaction.Domain.Entities.Transaction>(entity =>
            {
                entity.ToTable("Transaction");
                entity.HasKey(p => p.Id).HasName("PK_Transaction");

                entity.Property(p => p.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

                entity.Property(p => p.SourceAccountId)
                .HasColumnName("SourceAccountId")
                .ValueGeneratedNever();

                entity.Property(p => p.TargetAccountId)
                .HasColumnName("TargetAccountId")
                .ValueGeneratedNever();

                entity.Property(p => p.TransferTypeId)
                .HasColumnName("TransferTypeId")
                .ValueGeneratedNever();

                entity.Property(p => p.IdState)
                .HasColumnName("IdState");

                entity.Property(p => p.CreatedAd)
                .HasColumnName("CreatedAd");

            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
