using System.Collections.Generic;
using System.Linq;
using LiveClinic.Pharmacy.Core.Application.Inventory.Queries;
using LiveClinic.Pharmacy.Core.Domain.Inventory;
using LiveClinic.Pharmacy.Core.Tests.TestArtifacts;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Serilog;

namespace LiveClinic.Pharmacy.Core.Tests.Application.Queries
{
    [TestFixture]
    public class GetInventoryStatsTests
    {
        private IMediator _mediator;
        private List<Drug> _drugs;

        [OneTimeSetUp]
        public void Init()
        {
            _drugs = TestData.CreateTestDrugs();
            TestInitializer.SeedData(_drugs);
        }

        [SetUp]
        public void SetUp()
        {
            _mediator = TestInitializer.ServiceProvider.GetService<IMediator>();
        }
        [Test]
        public void should_Get_All()
        {
            var stats = _mediator.Send( new  GetInventoryStats()).Result.Value;
            Assert.NotNull(stats);
            Assert.True(stats.OutOfStockDrugStats.Any());
            Log.Debug(stats.ToString());
            Log.Debug("In stock");
            foreach (var drugStat in stats.InStockDrugStats)
                Log.Debug(drugStat.ToString());
            Log.Debug("Out of stock");
            foreach (var drugStat in stats.OutOfStockDrugStats)
                Log.Debug(drugStat.ToString());
        }
    }
}
