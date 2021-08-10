using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using LiveClinic.Contracts;
using LiveClinic.Pharmacy.Core.Domain.DrugAggregate.Events;
using LiveClinic.Pharmacy.Core.Domain.PrescriptionOrderAggregate;
using MassTransit;
using MediatR;

namespace LiveClinic.Pharmacy.Core.Application.EventHandlers
{
    //TODO: what next
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

            if (!notification.IsAvailable)
            {
                var ordEvent=_mapper.Map<OrderRejected>(prep);

                await _bus.Publish<OrderRejected>(ordEvent);
                return;
            }

            var ordAcpEvent=_mapper.Map<OrderRejected>(prep);

            await _bus.Publish<OrderAccepted>(ordAcpEvent);
        }
    }
}
