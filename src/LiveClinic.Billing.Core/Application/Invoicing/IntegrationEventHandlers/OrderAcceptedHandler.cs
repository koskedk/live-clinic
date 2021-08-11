using System.Threading.Tasks;
using AutoMapper;
using LiveClinic.Billing.Core.Application.Invoicing.Commands;
using LiveClinic.Billing.Core.Application.Invoicing.Dtos;
using LiveClinic.Contracts;
using MassTransit;
using MediatR;
using Serilog;

namespace LiveClinic.Billing.Core.Application.Invoicing.IntegrationEventHandlers
{
    public class OrderAcceptedHandler:IConsumer<OrderAccepted>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public OrderAcceptedHandler(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<OrderAccepted> context)
        {
            Log.Debug(new string('*',40));
            Log.Debug($"{context.Message.OrderNo}");
            Log.Debug(new string('#',40));

            var dto = _mapper.Map<OrderInvoiceDto>(context.Message);
            await _mediator.Send(new GenerateInvoice(dto));
        }
    }
}
