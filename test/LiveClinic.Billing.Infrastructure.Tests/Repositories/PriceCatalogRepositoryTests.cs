using LiveClinic.Billing.Core.Domain.PriceAggregate;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Serilog;

namespace LiveClinic.Billing.Infrastructure.Tests.Repositories
{
    [TestFixture]
    public class PriceCatalogRepositoryTests
    {
        private IPriceCatalogRepository _priceCatalogRepository;

        [SetUp]
        public void SetUp()
        {
            _priceCatalogRepository = TestInitializer.ServiceProvider.GetService<IPriceCatalogRepository>();
        }

        [Test]
        public void should_GetPrice()
        {
            var price = _priceCatalogRepository.GetPrice(x => x.DrugCode == "PN").Result;
            Assert.NotNull(price);
            Log.Debug(price.ToString());
        }
    }
}
