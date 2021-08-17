using System;
using System.Collections.Generic;
using System.Linq;
using LiveClinic.Contracts;
using LiveClinic.Pharmacy.Core.Application.Inventory.Queries;
using LiveClinic.Pharmacy.Core.Application.Orders.Commands;
using LiveClinic.Pharmacy.Core.Domain.Inventory;
using LiveClinic.Pharmacy.Core.Domain.Orders;
using LiveClinic.Pharmacy.Core.Tests.TestArtifacts;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Serilog;

namespace LiveClinic.Pharmacy.Core.Tests.Application.Orders.Commands
{
    [TestFixture]
    public class DispenseDrugTests
    {
        private IMediator _mediator;
        private Drug _drug;
        private PrescriptionOrder _order;


        [OneTimeSetUp]
        public void Init()
        {
            _drug = TestData.CreateTestDrugWithStock("YYY", 5);

            _order = TestData.CreateTestPrescriptionOrder(new List<Drug>() {_drug},1,1).First();
            _order.OrderItems.ForEach(x =>
            {
                x.DrugId = _drug.Id;
                x.Days = 1;
                x.Quantity = 1;
            });
            _order.PaymentId=Guid.NewGuid();

            TestInitializer.SeedData(new[] {_drug}, new[] {_order});
        }

        [SetUp]
        public void SetUp()
        {
            _mediator = TestInitializer.ServiceProvider.GetService<IMediator>();
        }
        [Test]
        public void should_Dispense()
        {

            var res = _mediator.Send( new  DispenseDrugs(_order.OrderId)).Result;
            Assert.True(res.IsSuccess);
            Assert.That(TestInitializer.TestConsumerOrderFulfilled.Consumed.Any<OrderFulfilled>().Result);

            var inventoryQuery = _mediator.Send(new GetInventory(_drug.Id)).Result;
            var inventoryDto = inventoryQuery.Value.First();
            Assert.AreEqual(4,inventoryDto.QuantityStock);
            Log.Debug(inventoryDto.ToString());
        }
    }
}
