using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LiveClinic.Consultation.Core.Application.Prescriptions.Commands;
using LiveClinic.Consultation.Core.Tests.TestArtifacts;
using LiveClinic.Contracts;
using MassTransit;
using MassTransit.Testing;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Serilog;

namespace LiveClinic.Consultation.Core.Tests.Application.Commands
{
    [TestFixture]
    public class PrescribeDrugsTests
    {
        private IMediator _mediator;
        private InMemoryTestHarness _harness;
        private ConsumerTestHarness<TestOrderGeneratedHandler> _consumerHarness;
        private OrderGenerated _orderGenerated;

        [SetUp]
        public async Task SetUp()
        {
            _mediator = TestInitializer.ServiceProvider.GetService<IMediator>();
            await  SetupBus();
        }
        [Test]
        public async Task should_PrescribeDrugs()
        {
            var dto = TestData.CreateTestPrescriptionDto();
            var res = _mediator.Send( new  PrescribeDrugs(dto)).Result;
            Assert.True(res.IsSuccess);
            Assert.That(await _consumerHarness.Consumed.Any<OrderGenerated>());
        }

        [TearDown]
        public  async Task TearDown()
        {
             await _harness.Stop();
        }

        private async Task SetupBus()
        {
            _harness = TestInitializer.ServiceProvider.GetService<InMemoryTestHarness>();
            _consumerHarness = _harness.Consumer<TestOrderGeneratedHandler>();
            await _harness.Start();
        }

        private class TestOrderGeneratedHandler:IConsumer<OrderGenerated>
        {
            public async Task Consume(ConsumeContext<OrderGenerated> context)
            {
                Assert.True(context.Message.OrderItems.Any());
                Log.Debug($"{context.Message.OrderNo}");
                foreach (var item in context.Message.OrderItems)
                    Log.Debug($"  > {item.DrugCode}|{item.Quantity}|{item.Days}");
            }
        }
    }
}
