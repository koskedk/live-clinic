using System.Linq;
using LiveClinic.Billing.Core.Domain.InvoiceAggregate;
using LiveClinic.Billing.Core.Domain.PriceAggregate;
using LiveClinic.Billing.Infrastructure.Seed;
using LiveClinic.SharedKernel.Common;
using LiveClinic.SharedKernel.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LiveClinic.Billing.Infrastructure.Persistence
{
    public class BillingDbContext:BaseContext
    {
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PriceCatalog> PriceCatalogs { get; set; }

        public BillingDbContext(DbContextOptions<BillingDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>().OwnsOne(
                p => p.AmountPaid,
                sa =>
                {
                    sa.Property(t => t.Amount).HasColumnName(nameof(Payment.AmountPaid));
                    sa.Property(t => t.Currency).HasColumnName(nameof(Money.Currency));
                }
            );
            modelBuilder.Entity<InvoiceItem>().OwnsOne(
                p => p.QuotePrice,
                sa =>
                {
                    sa.Property(t => t.Amount).HasColumnName(nameof(InvoiceItem.QuotePrice));
                    sa.Property(t => t.Currency).HasColumnName(nameof(Money.Currency));
                }
            );

            modelBuilder.Entity<PriceCatalog>().OwnsOne(
                p => p.UnitPrice,
                sa =>
                {
                    sa.Property(t => t.Amount).HasColumnName(nameof(PriceCatalog.UnitPrice));
                    sa.Property(t => t.Currency).HasColumnName(nameof(Money.Currency));
                }
            );
        }

        public override void EnsureSeeded()
        {
            if (!PriceCatalogs.Any())
                PriceCatalogs.AddRange(PriceCatalogSeed.GetCatalogs());

            SaveChanges();
        }
    }
}
