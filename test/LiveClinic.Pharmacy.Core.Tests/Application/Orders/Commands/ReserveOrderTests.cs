using System;
using System.Collections.Generic;
using System.Linq;
using LiveClinic.Pharmacy.Core.Application.Orders.Commands;
using LiveClinic.Pharmacy.Core.Domain.Orders;
using LiveClinic.Pharmacy.Core.Tests.TestArtifacts;
using LiveClinic.Pharmacy.Infrastructure;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LiveClinic.Pharmacy.Core.Tests.Application.Orders.Commands
{
    [TestFixture]
    public class ReserveOrderTests
    {
        private IMediator _mediator;
        private List<PrescriptionOrder> _orders;

        [OneTimeSetUp]
        public void Init()
        {
            _orders = TestData.CreateTestPrescriptionOrder(TestInitializer.ServiceProvider
                .GetService<PharmacyDbContext>().Drugs.ToList());
            _orders.ForEach(x => x.PaymentId = Guid.Empty);
            TestInitializer.SeedData(new[]{ _orders});
        }

        [SetUp]
        public void SetUp()
        {
            _mediator = TestInitializer.ServiceProvider.GetService<IMediator>();
        }
        [Test]
        public void should_Reserve_Stock()
        {
            var payId = Guid.NewGuid();
            var order = _orders.First();

            Assert.False(order.IsReserved);
            var res = _mediator.Send( new  ReserveOrder(order.OrderId,payId)).Result;
            Assert.True(res.IsSuccess);

            var context = TestInitializer.ServiceProvider.GetRequiredService<PharmacyDbContext>();
            var savedOrder = context.PrescriptionOrders.FirstOrDefault(x => x.OrderId == order.OrderId);
            Assert.AreEqual(payId, savedOrder.PaymentId);
            Assert.True(savedOrder.IsReserved);
        }
    }
}
