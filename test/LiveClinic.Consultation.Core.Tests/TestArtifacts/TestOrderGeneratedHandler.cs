using System.Linq;
using System.Threading.Tasks;
using LiveClinic.Contracts;
using MassTransit;
using NUnit.Framework;
using Serilog;

namespace LiveClinic.Consultation.Core.Tests.TestArtifacts
{
    public class TestOrderGeneratedHandler:IConsumer<OrderGenerated>
    {
        public Task Consume(ConsumeContext<OrderGenerated> context)
        {
            Assert.True(context.Message.OrderItems.Any());
            Log.Debug($"ORDER:{context.Message.OrderNo}");
            return Task.CompletedTask;
        }
    }
}
