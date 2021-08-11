using System.Threading.Tasks;
using LiveClinic.Contracts;
using MassTransit;
using NUnit.Framework;
using Serilog;

namespace LiveClinic.Billing.Core.Tests.TestArtifacts
{
    public class TestOrderPaidHandler:IConsumer<OrderPaid>
    {
        public  Task Consume(ConsumeContext<OrderPaid> context)
        {
            Assert.NotNull(context.Message.OrderId);
            Log.Debug($"Fulfilled: {context.Message.OrderId}");
            return Task.CompletedTask;
        }
    }
}
