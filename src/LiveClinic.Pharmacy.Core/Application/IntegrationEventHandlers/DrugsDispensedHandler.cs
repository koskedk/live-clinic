using System.Threading;
using System.Threading.Tasks;
using LiveClinic.Contracts;
using LiveClinic.Pharmacy.Core.Domain.Inventory.Events;
using LiveClinic.Pharmacy.Core.Domain.Orders;
using MassTransit;
using MediatR;

namespace LiveClinic.Pharmacy.Core.Application.IntegrationEventHandlers
{
    public class DrugDispensedHandler : INotificationHandler<DrugsDispensed>
    {
        private readonly IBus _bus;
        public DrugDispensedHandler(IBus bus)
        {
            _bus = bus;
        }

        public async Task Handle(DrugsDispensed notification, CancellationToken cancellationToken)
        {
            await _bus.Publish<OrderFulfilled>(new
            {
                OrderId = notification.OrderId, FulfillmentDate = notification.TimeStamp
            });
        }
    }
}
