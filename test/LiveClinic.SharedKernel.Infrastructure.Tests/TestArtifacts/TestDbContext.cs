using LiveClinic.SharedKernel.Infrastructure.Persistence;
using LiveClinic.SharedKernel.Infrastructure.Tests.TestArtifacts.Model;
using Microsoft.EntityFrameworkCore;

namespace LiveClinic.SharedKernel.Infrastructure.Tests.TestArtifacts
{
    public class TestDbContext : BaseContext
    {
        public DbSet<TestCar> TestCars { get; set; }
        public DbSet<TestCarModel> TestCarModels { get; set; }

        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TestCarModel>().OwnsOne(p => p.Trim, sa =>
            {
                sa.Property(t => t.FuelType).HasColumnName(nameof(TestTrim.FuelType));
                sa.Property(t => t.Transmission).HasColumnName(nameof(TestTrim.Transmission));
            });
        }
    }
}
