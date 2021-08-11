using System.Linq;
using LiveClinic.Pharmacy.Core.Domain.Inventory;
using LiveClinic.Pharmacy.Core.Domain.Orders;
using LiveClinic.Pharmacy.Infrastructure.Seed;
using LiveClinic.SharedKernel.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LiveClinic.Pharmacy.Infrastructure
{
    public class PharmacyDbContext:BaseContext
    {
        public DbSet<Drug> Drugs { get; set; }
        public DbSet<StockTransaction> StockTransactions { get; set; }

        public DbSet<PrescriptionOrder> PrescriptionOrders { get; set; }
        public DbSet<PrescriptionOrderItem> PrescriptionOrderItems{ get; set; }

        public PharmacyDbContext(DbContextOptions<PharmacyDbContext> options) : base(options)
        {
        }

        public override void EnsureSeeded()
        {
            if (!Drugs.Any())
            {
                Drugs.AddRange(DrugSeed.GetDrugs());
                SaveChanges();
            }
        }
    }
}
