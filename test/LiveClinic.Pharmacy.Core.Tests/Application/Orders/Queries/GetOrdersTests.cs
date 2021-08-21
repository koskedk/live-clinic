using System.Collections.Generic;
using System.Linq;
using LiveClinic.Pharmacy.Core.Application.Orders.Queries;
using LiveClinic.Pharmacy.Core.Domain.Orders;
using LiveClinic.Pharmacy.Core.Tests.TestArtifacts;
using LiveClinic.Pharmacy.Infrastructure;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LiveClinic.Pharmacy.Core.Tests.Application.Orders.Queries
{
    [TestFixture]
    public class GetOrdersTests
    {
        private IMediator _mediator;
        private List<PrescriptionOrder> _orders;

        [OneTimeSetUp]
        public void Init()
        {
            _orders = TestData.CreateTestPrescriptionOrder(TestInitializer.ServiceProvider
                .GetService<PharmacyDbContext>().Drugs.ToList());
            TestInitializer.SeedData(_orders);
        }

        [SetUp]
        public void SetUp()
        {
            _mediator = TestInitializer.ServiceProvider.GetService<IMediator>();
        }
        [Test]
        public void should_Get_All()
        {
            var stats = _mediator.Send( new  GetOrders(PrescriptionStatus.Active)).Result;
            Assert.NotNull(stats.IsSuccess);
            Assert.True(stats.Value.Any());
        }
    }
}
