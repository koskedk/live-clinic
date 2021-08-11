using System.Linq;
using LiveClinic.Billing.Core.Application.Invoicing.Commands;
using LiveClinic.Billing.Core.Application.Invoicing.Dtos;
using LiveClinic.Billing.Core.Domain.InvoiceAggregate;
using LiveClinic.Billing.Core.Tests.TestArtifacts;
using LiveClinic.Billing.Infrastructure.Persistence;
using LiveClinic.Contracts;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LiveClinic.Billing.Core.Tests.Application.Commands
{
    [TestFixture]
    public class ReceivePaymentTests
    {
        private IMediator _mediator;
        private Invoice _invoice;

        [OneTimeSetUp]
        public void Init()
        {
            var catalogs = TestInitializer.ServiceProvider.GetService<BillingDbContext>().PriceCatalogs.ToList();
            _invoice = TestData.GenerateInvoices(catalogs).First();
            TestInitializer.SeedData(new []{_invoice});
        }

        [SetUp]
        public void SetUp()
        {
            _mediator = TestInitializer.ServiceProvider.GetService<IMediator>();
        }

        [Test]
        public void should_ReceivePay()
        {
            var pay = new PaymentDto()
            {
                InvoiceId = _invoice.Id,
               Amount   = 300
            };
            var res = _mediator.Send(new ReceivePayment(pay)).Result;
            Assert.True(res.IsSuccess);
            Assert.That(TestInitializer.TestConsumerOrderPaid.Consumed.Any<OrderPaid>().Result);
        }
    }
}
