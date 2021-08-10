using System.Linq;
using LiveClinic.Billing.Core.Domain.InvoiceAggregate;
using LiveClinic.Billing.Infrastructure.Persistence;
using LiveClinic.Billing.Infrastructure.Tests.TestArtifacts;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Serilog;

namespace LiveClinic.Billing.Infrastructure.Tests.Repositories
{
    [TestFixture]
    public class InvoiceRepositoryTests
    {
        private IInvoiceRepository _invoiceRepository;

        [OneTimeSetUp]
        public void Init()
        {
            var catalogs = TestInitializer.ServiceProvider.GetService<BillingDbContext>().PriceCatalogs.ToList();
            TestInitializer.SeedData(TestData.GenerateInvoices(catalogs));
        }

        [SetUp]
        public void SetUp()
        {
            _invoiceRepository = TestInitializer.ServiceProvider.GetService<IInvoiceRepository>();
        }

        [Test]
        public void should_Load_All_Invoices()
        {
            var invoices = _invoiceRepository.LoadAll().ToList();
            Assert.True(invoices.Count > 0);
            foreach (var invoice in invoices)
                Log.Debug(invoice.ToString());
        }
    }
}
