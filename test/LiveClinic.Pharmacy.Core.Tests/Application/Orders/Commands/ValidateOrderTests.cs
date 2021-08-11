using System.Collections.Generic;
using System.Linq;
using LiveClinic.Contracts;
using LiveClinic.Pharmacy.Core.Application.Orders.Commands;
using LiveClinic.Pharmacy.Core.Domain.Inventory;
using LiveClinic.Pharmacy.Core.Domain.Orders;
using LiveClinic.Pharmacy.Core.Tests.TestArtifacts;
using LiveClinic.Pharmacy.Infrastructure;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LiveClinic.Pharmacy.Core.Tests.Application.Orders.Commands
{
    [TestFixture]
    public class ValidateOrderTests
    {
        private IMediator _mediator;
        private List<PrescriptionOrder> _orders;
        private Drug _drug;

        [OneTimeSetUp]
        public void Init()
        {
            _orders = TestData.CreateTestPrescriptionOrder(TestInitializer.ServiceProvider
                .GetService<PharmacyDbContext>().Drugs.ToList());
            _drug = TestData.CreateTestDrugWithStock("YYY", 100);
            TestInitializer.SeedData(new[]{ _drug});
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
            Assert.That(TestInitializer.TestConsumerOrderRejected.Consumed.Any<OrderRejected>().Result);

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
            Assert.That(TestInitializer.TestConsumerOrderAccepted.Consumed.Any<OrderAccepted>().Result);

            var context = TestInitializer.ServiceProvider.GetRequiredService<PharmacyDbContext>();
            var savedOrder = context.PrescriptionOrders.FirstOrDefault(x => x.OrderId == order.OrderId);
            Assert.NotNull(savedOrder);
        }
    }
}
