using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LiveClinic.Contracts;
using LiveClinic.Pharmacy.Core.Application.Commands;
using LiveClinic.Pharmacy.Core.Domain.DrugAggregate;
using LiveClinic.Pharmacy.Core.Domain.DrugAggregate.Events;
using LiveClinic.Pharmacy.Core.Domain.PrescriptionOrderAggregate;
using LiveClinic.Pharmacy.Core.Tests.TestArtifacts;
using LiveClinic.Pharmacy.Infrastructure;
using MassTransit;
using MassTransit.Testing;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Serilog;

namespace LiveClinic.Pharmacy.Core.Tests.Application.Commands
{
    [TestFixture]
    public class ValidateOrderTests
    {
        private IMediator _mediator;
        private List<PrescriptionOrder> _orders;
        private Drug _drug;
        private InMemoryTestHarness _harness;
        private ConsumerTestHarness<TestOrderAcceptedHandler> _consumerOrderAccepted;
        private ConsumerTestHarness<TestOrderRejectedHandler> _consumerOrderRejected;

        [OneTimeSetUp]
        public void Init()
        {
            _orders = TestData.CreateTestPrescriptionOrder(TestInitializer.ServiceProvider
                .GetService<PharmacyDbContext>().Drugs.ToList());
            _drug = TestData.CreateTestDrugWithStock("YYY", 100);
            TestInitializer.SeedData(new[]{ _drug});
            SetupBus().Wait();
        }

        [SetUp]
        public void SetUp()
        {
            _mediator = TestInitializer.ServiceProvider.GetService<IMediator>();
        }

        [Test]
        public void should_Validate_Order()
        {
            var order = _orders.First();
            var res = _mediator.Send( new  ValidateOrder(order)).Result;
            Assert.True(res.IsSuccess);
            Assert.That(_consumerOrderRejected.Consumed.Any<OrderRejected>().Result);

            var context = TestInitializer.ServiceProvider.GetRequiredService<PharmacyDbContext>();
            var savedOrder = context.PrescriptionOrders.FirstOrDefault(x => x.OrderId == order.OrderId);
            Assert.NotNull(savedOrder);
        }

        [Test]
        public void should_Validate_Order_InStock()
        {
            var order = _orders.Last();
            order.OrderItems.ForEach(x => x.DrugCode = _drug.Code);
            var res = _mediator.Send( new  ValidateOrder(order)).Result;
            Assert.True(res.IsSuccess);
            Assert.That(_consumerOrderAccepted.Consumed.Any<OrderAccepted>().Result);

            var context = TestInitializer.ServiceProvider.GetRequiredService<PharmacyDbContext>();
            var savedOrder = context.PrescriptionOrders.FirstOrDefault(x => x.OrderId == order.OrderId);
            Assert.NotNull(savedOrder);
        }

        [OneTimeTearDown]
        public  async Task TearDown()
        {
            await _harness.Stop();
        }

        private async Task SetupBus()
        {
            _harness = TestInitializer.ServiceProvider.GetService<InMemoryTestHarness>();
            _consumerOrderAccepted = _harness.Consumer<TestOrderAcceptedHandler>();
            _consumerOrderRejected = _harness.Consumer<TestOrderRejectedHandler>();
            await _harness.Start();
        }
    }
}
