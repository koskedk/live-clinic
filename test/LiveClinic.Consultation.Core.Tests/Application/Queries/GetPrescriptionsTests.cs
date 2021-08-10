using System.Collections.Generic;
using System.Linq;
using LiveClinic.Consultation.Core.Application.Prescriptions.Queries;
using LiveClinic.Consultation.Core.Domain.Prescriptions;
using LiveClinic.Consultation.Core.Tests.TestArtifacts;
using MediatR;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;
using Serilog;


namespace LiveClinic.Consultation.Core.Tests.Application.Queries
{
    [TestFixture]
    public class GetPrescriptionsTests
    {
        private List<Prescription> _prescriptions = new List<Prescription>();
        private IMediator _mediator;

        [OneTimeSetUp]
        public void Init()
        {
            _prescriptions = TestData.CreateTestPrescriptions();
            TestInitializer.SeedData(_prescriptions);
        }

        [SetUp]
        public void SetUp()
        {
            _mediator = TestInitializer.ServiceProvider.GetService<IMediator>();
        }

        [Test]
        public void should_Get_All()
        {
            var res = _mediator.Send(new GetPrescriptions()).Result;
            Assert.True(res.IsSuccess);
            Assert.True(res.Value.Any());

            foreach (var prescription in res.Value)
            {
                Log.Debug($"{prescription}");
                foreach (var medication in prescription.Medications)
                    Log.Debug($"    {medication}");
            }
        }

        [Test]
        public void should_Get_By_Patient()
        {
            var res = _mediator.Send(new GetPrescriptions(null,_prescriptions.First().Patient)).Result;
            Assert.True(res.IsSuccess);
            Assert.True(res.Value.Any());

            foreach (var prescription in res.Value)
            {
                Log.Debug($"{prescription}");
                foreach (var medication in prescription.Medications)
                    Log.Debug($"    {medication}");
            }
        }

        [Test]
        public void should_Get_Prescription()
        {
            var res = _mediator.Send(new GetPrescriptions(_prescriptions.First().Id)).Result;
            Assert.True(res.IsSuccess);
            Assert.True(res.Value.Count == 1);

            foreach (var prescription in res.Value)
            {
                Log.Debug($"{prescription}");
                foreach (var medication in prescription.Medications)
                    Log.Debug($"    {medication}");
            }
        }
    }
}
