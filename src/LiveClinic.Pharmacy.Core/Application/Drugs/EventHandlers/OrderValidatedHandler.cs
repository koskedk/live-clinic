using System.Threading;
using System.Threading.Tasks;
using LiveClinic.Contracts;
using LiveClinic.Pharmacy.Core.Domain.DrugAggregate.Events;
using MassTransit;
using MediatR;

namespace LiveClinic.Pharmacy.Core.Application.EventHandlers
{
    public class OrderValidatedHandler : INotificationHandler<OrderValidated>
    {
        private readonly IBus _bus;

        public OrderValidatedHandler(IBus bus)
        {
            _bus = bus;
        }

        public async Task Handle(OrderValidated notification, CancellationToken cancellationToken)
        {
            if (!notification.IsAvailable)
                return;

            await _bus.Publish<DrugOrderValidated>(notification.DrugOrder);
        }
    }
}
