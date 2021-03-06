using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using LiveClinic.Contracts;
using LiveClinic.Pharmacy.Core.Domain.Orders;
using LiveClinic.Pharmacy.Core.Domain.Orders.Events;
using MassTransit;
using MediatR;

namespace LiveClinic.Pharmacy.Core.Application.IntegrationEventHandlers
{
    public class OrderValidatedHandler : INotificationHandler<OrderValidated>
    {
        private readonly IBus _bus;
        private readonly IPrescriptionOrderRepository _prescriptionOrderRepository;
        private readonly IMapper _mapper;

        public OrderValidatedHandler(IBus bus, IPrescriptionOrderRepository prescriptionOrderRepository, IMapper mapper)
        {
            _bus = bus;
            _prescriptionOrderRepository = prescriptionOrderRepository;
            _mapper = mapper;
        }

        public async Task Handle(OrderValidated notification, CancellationToken cancellationToken)
        {
            var prep = _prescriptionOrderRepository
                .LoadAll(x => x.Id == notification.PrescriptionOrderId)
                .FirstOrDefault();

            if (null == prep)
                throw new Exception("Order not found");

            if (notification.IsAvailable)
            {
                var orderAccepted=_mapper.Map<OrderAccepted>(prep);
                await _bus.Publish<OrderAccepted>(orderAccepted);
                return;
            }

            var orderRejected=_mapper.Map<OrderRejected>(prep);
            await _bus.Publish<OrderRejected>(orderRejected);
        }
    }
}
