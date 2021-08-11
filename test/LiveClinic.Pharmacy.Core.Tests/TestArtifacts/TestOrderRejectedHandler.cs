using System.Linq;
using System.Threading.Tasks;
using LiveClinic.Contracts;
using MassTransit;
using NUnit.Framework;
using Serilog;

namespace LiveClinic.Pharmacy.Core.Tests.TestArtifacts
{
    public class TestOrderRejectedHandler:IConsumer<OrderRejected>
    {
        public async Task Consume(ConsumeContext<OrderRejected> context)
        {
            Assert.True(context.Message.OrderItems.Any());
            Log.Debug($"Rejected: {context.Message.OrderNo}");
        }
    }
}