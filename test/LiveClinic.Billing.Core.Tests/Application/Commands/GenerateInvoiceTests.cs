using System.Linq;
using LiveClinic.Billing.Core.Application.Invoicing.Commands;
using LiveClinic.Billing.Core.Application.Invoicing.Dtos;
using LiveClinic.Billing.Core.Tests.TestArtifacts;
using LiveClinic.Billing.Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LiveClinic.Billing.Core.Tests.Application.Commands
{
    [TestFixture]
    public class GenerateInvoiceTests
    {
        private IMediator _mediator;
        private OrderInvoiceDto _invoiceDto;

        [OneTimeSetUp]
        public void Init()
        {
            var catalogs = TestInitializer.ServiceProvider.GetService<BillingDbContext>().PriceCatalogs.ToList();
            _invoiceDto = TestData.GenerateOrderInvoiceDtos(catalogs).First();
        }

        [SetUp]
        public void SetUp()
        {
            _mediator = TestInitializer.ServiceProvider.GetService<IMediator>();
        }
        [Test]
        public void should_GenerateInvoice()
        {
            var res = _mediator.Send(new GenerateInvoice(_invoiceDto)).Result;
            Assert.True(res.IsSuccess);
        }
    }
}
