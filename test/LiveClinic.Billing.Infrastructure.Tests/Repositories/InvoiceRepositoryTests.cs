using System.Collections.Generic;
using System.Linq;
using LiveClinic.Billing.Core.Domain.InvoiceAggregate;
using LiveClinic.Billing.Infrastructure.Persistence;
using LiveClinic.Billing.Infrastructure.Tests.TestArtifacts;
using LiveClinic.SharedKernel.Common;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Serilog;

namespace LiveClinic.Billing.Infrastructure.Tests.Repositories
{
    [TestFixture]
    public class InvoiceRepositoryTests
    {
        private IInvoiceRepository _invoiceRepository;
        private List<Invoice> _invoices;

        [OneTimeSetUp]
        public void Init()
        {
            var catalogs = TestInitializer.ServiceProvider.GetService<BillingDbContext>().PriceCatalogs.ToList();
            _invoices = TestData.GenerateInvoices(catalogs);
            TestInitializer.SeedData(_invoices);
        }

        [SetUp]
        public void SetUp()
        {
            _invoiceRepository = TestInitializer.ServiceProvider.GetService<IInvoiceRepository>();
        }

        [Test]
        public void should_Save_Payment()
        {
            var pendingInvoice = _invoices.Last();
            var payment = new Payment(Money.FromAmount(pendingInvoice.Balance.Amount), pendingInvoice.Id);
            Assert.True(pendingInvoice.Status==InvoiceStatus.NotPaid);

            _invoiceRepository.UpdatePayments(pendingInvoice.Id,payment);

            var ctx = TestInitializer.ServiceProvider.GetService<BillingDbContext>();
            var invoice = ctx.Invoices.Find(pendingInvoice.Id);
            Assert.True(pendingInvoice.Status==InvoiceStatus.Paid);
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
