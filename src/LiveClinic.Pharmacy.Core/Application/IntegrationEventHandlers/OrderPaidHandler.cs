using System.Threading.Tasks;
using AutoMapper;
using LiveClinic.Contracts;
using LiveClinic.Pharmacy.Core.Application.Orders.Commands;
using MassTransit;
using MediatR;
using Serilog;

namespace LiveClinic.Pharmacy.Core.Application.IntegrationEventHandlers
{
    public class OrderPaidHandler:IConsumer<OrderPaid>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public OrderPaidHandler(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<OrderPaid> context)
        {
            Log.Debug(new string('*',40));
            Log.Debug($"{context.Message.PaymentId}");
            Log.Debug(new string('#',40));

            await _mediator.Send(new ReserveOrder(context.Message.OrderId,context.Message.PaymentId));
        }
    }
}
