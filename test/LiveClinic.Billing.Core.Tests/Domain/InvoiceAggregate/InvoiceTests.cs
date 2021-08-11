using System.Collections.Generic;
using System.Linq;
using LiveClinic.Billing.Core.Domain.InvoiceAggregate;
using LiveClinic.Billing.Core.Domain.PriceAggregate;
using LiveClinic.Billing.Core.Tests.TestArtifacts;
using LiveClinic.Billing.Infrastructure.Persistence;
using LiveClinic.SharedKernel.Common;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LiveClinic.Billing.Core.Tests.Domain.InvoiceAggregate
{
    [TestFixture]
    public class InvoiceTests
    {
        private List<PriceCatalog> _catalogs;
        private List<Invoice> _invoices;

        [SetUp]
        public void SetUp()
        {
            _catalogs = TestInitializer.ServiceProvider.GetService<BillingDbContext>().PriceCatalogs.ToList();
            _invoices = TestData.GenerateInvoices(_catalogs);
        }
        [Test]
        public void should_Calc_Amounts()
        {
            var invoice = _invoices.First();
            Assert.True(invoice.TotalAmount.Amount>0);
        }

        [Test]
        public void should_Update_Status()
        {
            var invoice = _invoices.Last();
            invoice.MakePayment(new Payment(new Money(120,"KES"),invoice.Id));
            Assert.True(invoice.Balance.Amount>0);
            Assert.True(invoice.Status==InvoiceStatus.NotPaid);

            invoice.MakePayment(new Payment(new Money(180,"KES"),invoice.Id));
            Assert.True(invoice.Balance.Amount==0);
            Assert.True(invoice.Status==InvoiceStatus.Paid);
        }
    }
}
