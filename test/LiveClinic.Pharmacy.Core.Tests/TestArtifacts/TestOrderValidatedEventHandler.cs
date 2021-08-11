using System.Threading;
using System.Threading.Tasks;
using LiveClinic.Pharmacy.Core.Domain.Orders.Events;
using MediatR;
using NUnit.Framework;
using Serilog;

namespace LiveClinic.Pharmacy.Core.Tests.TestArtifacts
{
    public class TestOrderValidatedEventHandler : INotificationHandler<OrderValidated>
    {
        public Task Handle(OrderValidated notification, CancellationToken cancellationToken)
        {
            Assert.NotNull(notification.PrescriptionOrderId);
            Log.Debug($"{notification.PrescriptionOrderId}:{notification.IsAvailable}");
            return Task.CompletedTask;
        }
    }
}
