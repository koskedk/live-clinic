using System.Collections.Generic;
using System.Linq;
using LiveClinic.Billing.Core.Application.Invoicing.Queries;
using LiveClinic.Billing.Core.Domain.InvoiceAggregate;
using LiveClinic.Billing.Core.Tests.TestArtifacts;
using LiveClinic.Billing.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Serilog;

namespace LiveClinic.Billing.Core.Tests.Application.Queries
{
    [TestFixture]
    public class GetPatientInvoiceTests
    {
        private IMediator _mediator;
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
            _mediator = TestInitializer.ServiceProvider.GetService<IMediator>();
        }
        [Test]
        public void should_GetInvoice()
        {
            var res = _mediator.Send(new GetPatientInvoice(_invoices.First().Patient)).Result;
            Assert.True(res.IsSuccess);
            foreach (var invoice in res.Value)
            {
                Log.Debug($"{invoice}");
                foreach (var it in invoice.Items)
                {
                    Log.Debug($"{it}");
                }
                foreach (var p in invoice.Payments)
                {
                    Log.Debug($"{p}");
                }
            }
        }
    }
}
