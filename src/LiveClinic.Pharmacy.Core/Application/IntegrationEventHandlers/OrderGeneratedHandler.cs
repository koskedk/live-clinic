using System.Threading.Tasks;
using AutoMapper;
using LiveClinic.Contracts;
using LiveClinic.Pharmacy.Core.Application.Commands;
using LiveClinic.Pharmacy.Core.Domain.PrescriptionOrderAggregate;
using MassTransit;
using MediatR;
using Serilog;

namespace LiveClinic.Pharmacy.Core.Application.EventHandlers
{
    public class OrderGeneratedHandler:IConsumer<OrderGenerated>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public OrderGeneratedHandler(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<OrderGenerated> context)
        {
            Log.Debug(new string('*',40));
            Log.Debug($"{context.Message.OrderNo}");
            Log.Debug(new string('#',40));

            var order = _mapper.Map<PrescriptionOrder>(context.Message);

            await _mediator.Send(new ValidateOrder(order));
        }
    }
}
