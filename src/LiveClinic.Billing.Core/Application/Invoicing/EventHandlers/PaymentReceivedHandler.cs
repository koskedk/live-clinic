using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using LiveClinic.Billing.Core.Domain.InvoiceAggregate.Events;
using LiveClinic.Contracts;
using MassTransit;
using MediatR;

namespace LiveClinic.Billing.Core.Application.Invoicing.EventHandlers
{
    public class PaymentReceivedHandler : INotificationHandler<PaymentReceived>
    {
        private readonly IBus _bus;
        private readonly IMapper _mapper;

        public PaymentReceivedHandler(IBus bus, IMapper mapper)
        {
            _bus = bus;

            _mapper = mapper;
        }

        public async Task Handle(PaymentReceived notification, CancellationToken cancellationToken)
        {
            await _bus.Publish<DrugOrderPaid>(new
            {
                InvoiceId=notification.InvoiceId,
                OrderId=notification.OrderId
            },cancellationToken);





        }
    }
}
