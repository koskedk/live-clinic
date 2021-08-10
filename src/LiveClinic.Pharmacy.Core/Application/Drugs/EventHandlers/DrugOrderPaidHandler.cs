using System.Threading.Tasks;
using AutoMapper;
using LiveClinic.Contracts;
using LiveClinic.Pharmacy.Core.Application.Commands;
using MassTransit;
using MediatR;
using Serilog;

namespace LiveClinic.Pharmacy.Core.Application.EventHandlers
{
    public class DrugOrderPaidHandler:IConsumer<DrugOrderPaid>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public DrugOrderPaidHandler(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<DrugOrderPaid> context)
        {
            Log.Debug(new string('*',40));
            Log.Debug($"{context.Message.InvoiceId}");
            Log.Debug(new string('#',40));

            await _mediator.Send(new AllowDispense(context.Message.OrderId,context.Message.InvoiceId));
        }
    }
}
