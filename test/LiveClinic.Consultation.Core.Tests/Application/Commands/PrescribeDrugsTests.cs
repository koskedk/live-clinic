using System.Threading.Tasks;
using LiveClinic.Consultation.Core.Application.Prescriptions.Commands;
using LiveClinic.Consultation.Core.Tests.TestArtifacts;
using LiveClinic.Contracts;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace LiveClinic.Consultation.Core.Tests.Application.Commands
{
    [TestFixture]
    public class PrescribeDrugsTests
    {
        private IMediator _mediator;

        [SetUp]
        public void SetUp()
        {
            _mediator = TestInitializer.ServiceProvider.GetService<IMediator>();

        }

        [Test]
        public async Task should_PrescribeDrugs()
        {
            var dto = TestData.CreateTestPrescriptionDto();
            var res = _mediator.Send(new PrescribeDrugs(dto)).Result;
            Assert.True(res.IsSuccess);
            Assert.That(await TestInitializer.TestConsumerOrderGenerated.Consumed.Any<OrderGenerated>());
        }
    }
}
