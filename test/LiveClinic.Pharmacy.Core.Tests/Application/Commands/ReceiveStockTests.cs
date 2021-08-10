using System.Collections.Generic;
using System.Linq;
using LiveClinic.Pharmacy.Core.Application.Commands;
using LiveClinic.Pharmacy.Core.Application.Dtos;
using LiveClinic.Pharmacy.Core.Application.Queries;
using LiveClinic.Pharmacy.Core.Domain.DrugAggregate;
using LiveClinic.Pharmacy.Core.Tests.TestArtifacts;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Serilog;

namespace LiveClinic.Pharmacy.Core.Tests.Application.Commands
{
    [TestFixture]
    public class ReceiveStockTests
    {
        private IMediator _mediator;
        private Drug _drug;

        [OneTimeSetUp]
        public void Init()
        {
            _drug = TestData.CreateTestDrugWithStock("XYZ", 11);
            TestInitializer.SeedData(new[]{ _drug});
        }

        [SetUp]
        public void SetUp()
        {
            _mediator = TestInitializer.ServiceProvider.GetService<IMediator>();
        }
        [Test]
        public void should_Receive_Stock()
        {
            var stocks = new List<NewStockDto>
            {
                new NewStockDto() {DrugId = _drug.Id, BatchNo = "LEO", Quantity = 9, OrderRef = "POX"}
            };

            var res = _mediator.Send( new  ReceiveStock(stocks)).Result;
            Assert.True(res.IsSuccess);

            var inventoryQuery = _mediator.Send(new GetInventory(_drug.Id)).Result;
            var inventoryDto = inventoryQuery.Value.First();
            Assert.AreEqual(20,inventoryDto.QuantityStock);
            Log.Debug(inventoryDto.ToString());
        }
    }
}
