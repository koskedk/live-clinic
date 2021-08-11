using System.Threading.Tasks;
using LiveClinic.Contracts;
using MassTransit;
using NUnit.Framework;
using Serilog;

namespace LiveClinic.Pharmacy.Core.Tests.TestArtifacts
{
    public class TestOrderFulfilledHandler:IConsumer<OrderFulfilled>
    {
        public  Task Consume(ConsumeContext<OrderFulfilled> context)
        {
            Assert.NotNull(context.Message.OrderId);
            Log.Debug($"Fulfilled: {context.Message.OrderId}");
            return Task.CompletedTask;
        }
    }
}
