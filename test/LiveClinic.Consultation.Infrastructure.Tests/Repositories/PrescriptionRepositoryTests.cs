using System.Linq;
using LiveClinic.Consultation.Core.Domain.Prescriptions;
using LiveClinic.Consultation.Infrastructure.Tests.TestArtifacts;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Serilog;

namespace LiveClinic.Consultation.Infrastructure.Tests.Repositories
{
    [TestFixture]
    public class PrescriptionRepositoryTests
    {
        private IPrescriptionRepository _prescriptionRepository;

        [SetUp]
        public void SetUp()
        {
            TestInitializer.SeedData(TestData.CreateTestPrescriptions());
            _prescriptionRepository = TestInitializer.ServiceProvider.GetService<IPrescriptionRepository>();
        }

        [Test]
        public void should_Load_All()
        {
            var drugs = _prescriptionRepository.LoadAll().ToList();
            Assert.True(drugs.Count > 0);
            foreach (var drug in drugs)
                Log.Debug(drug.ToString());
        }
    }
}
