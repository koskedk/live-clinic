using System.Collections.Generic;
using System.Linq;
using LiveClinic.Consultation.Core.Application.Prescriptions.Commands;
using LiveClinic.Consultation.Core.Domain.Prescriptions;
using LiveClinic.Consultation.Core.Tests.TestArtifacts;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LiveClinic.Consultation.Core.Tests.Application.Commands
{
    [TestFixture]
    public class ChangePrescriptionStatusTests
    {
        private IMediator _mediator;
        private List<Prescription> _prescriptions;

        [SetUp]
        public void SetUp()
        {
            _prescriptions = TestData.CreateTestPrescriptions();
            TestInitializer.SeedData(_prescriptions);
            _mediator = TestInitializer.ServiceProvider.GetService<IMediator>();
        }
        [Test]
        public void should_Change_Status()
        {
            var pres = _prescriptions.First();
            var res = _mediator.Send( new  ChangePrescriptionStatus(pres.Id,OrderStatus.Fulfilled)).Result;
            Assert.True(res.IsSuccess);
        }
    }
}
