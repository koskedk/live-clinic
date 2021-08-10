using LiveClinic.Pharmacy.Core.Domain.DrugAggregate;
using LiveClinic.Pharmacy.Core.Tests.TestArtifacts;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LiveClinic.Pharmacy.Core.Tests.Application.Commands
{
    [TestFixture]
    public class DispenseDrugTests
    {
        private IMediator _mediator;
        private Drug _drug;

        [OneTimeSetUp]
        public void Init()
        {
            _drug = TestData.CreateTestDrugWithStock("YYY", 5);
            TestInitializer.SeedData(new[]{ _drug});
        }

        [SetUp]
        public void SetUp()
        {
            _mediator = TestInitializer.ServiceProvider.GetService<IMediator>();
        }
        // [Test]
        // public void should_Dispense_InStock()
        // {
        //     var res = _mediator.Send( new  DispenseDrug(_drug.Id,"LEO",4)).Result;
        //     Assert.True(res.IsSuccess);
        //
        //     var inventoryQuery = _mediator.Send(new GetInventory(_drug.Id)).Result;
        //     var inventoryDto = inventoryQuery.Value.First();
        //     Assert.AreEqual(1,inventoryDto.QuantityStock);
        //     Log.Debug(inventoryDto.ToString());
        // }
    }
}
