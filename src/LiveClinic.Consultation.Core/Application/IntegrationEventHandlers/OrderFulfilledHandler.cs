using System.Threading.Tasks;
using LiveClinic.Consultation.Core.Application.Prescriptions.Commands;
using LiveClinic.Consultation.Core.Domain.Prescriptions;
using LiveClinic.Contracts;
using MassTransit;
using MediatR;
using Serilog;

namespace LiveClinic.Consultation.Core.Application.IntegrationEventHandlers
{
    public class OrderFulfilledHandler:IConsumer<OrderFulfilled>
    {
        private readonly IMediator _mediator;

        public OrderFulfilledHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<OrderFulfilled> context)
        {
            Log.Debug(new string('*',40));
            Log.Debug($"FULFILLED:{context.Message.OrderId}");
            Log.Debug(new string('#',40));

            await _mediator.Send(new ChangePrescriptionStatus(context.Message.OrderId, OrderStatus.Fulfilled));
        }
    }
}
