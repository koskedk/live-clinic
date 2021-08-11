using System.Linq;
using System.Threading.Tasks;
using LiveClinic.Contracts;
using MassTransit;
using NUnit.Framework;
using Serilog;

namespace LiveClinic.Pharmacy.Core.Tests.TestArtifacts
{
    public class TestOrderAcceptedHandler:IConsumer<OrderAccepted>
    {
        public async Task Consume(ConsumeContext<OrderAccepted> context)
        {
            Assert.True(context.Message.OrderItems.Any());
            Log.Debug($"Accepted: {context.Message.OrderNo}");
        }
    }
}