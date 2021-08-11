using System.Linq;
using LiveClinic.Pharmacy.Core.Domain.DrugAggregate;
using LiveClinic.Pharmacy.Core.Domain.PrescriptionOrderAggregate;
using LiveClinic.Pharmacy.Infrastructure.Tests.TestArtifacts;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace LiveClinic.Pharmacy.Infrastructure.Tests.Repositories
{
    [TestFixture]
    public class PrescriptionOrderRepositoryTests
    {
        private IPrescriptionOrderRepository _prescriptionOrderRepository;

        [SetUp]
        public void SetUp()
        {
            TestInitializer.SeedData(TestData.CreateTestPrescriptionOrder(TestInitializer.ServiceProvider.GetService<PharmacyDbContext>().Drugs.ToList()));
            _prescriptionOrderRepository = TestInitializer.ServiceProvider.GetService<IPrescriptionOrderRepository>();
        }

        [Test]
        public void should_Load_All_Drugs()
        {
            var prescriptionOrders = _prescriptionOrderRepository.LoadAll().ToList();
            Assert.True(prescriptionOrders.Any());
            Assert.True(prescriptionOrders.First().OrderItems.Any());
        }
    }
}
