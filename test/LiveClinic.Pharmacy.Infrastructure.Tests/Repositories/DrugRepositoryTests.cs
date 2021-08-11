using System.Linq;
using LiveClinic.Pharmacy.Core.Domain.DrugAggregate;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace LiveClinic.Pharmacy.Infrastructure.Tests.Repositories
{
    [TestFixture]
    public class DrugRepositoryTests
    {
        private IDrugRepository _drugRepository;

        [SetUp]
        public void SetUp()
        {
            _drugRepository = TestInitializer.ServiceProvider.GetService<IDrugRepository>();
        }

        [Test]
        public void should_Load_All_Drugs()
        {
            var drugs = _drugRepository.LoadAll().ToList();
            Assert.True(drugs.Count > 0);
            foreach (var drug in drugs)
                Log.Debug(drug.ToString());
        }
    }
}
