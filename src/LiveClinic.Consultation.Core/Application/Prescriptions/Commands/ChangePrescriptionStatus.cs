using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using LiveClinic.Consultation.Core.Domain.Prescriptions;
using MediatR;
using Serilog;

namespace LiveClinic.Consultation.Core.Application.Prescriptions.Commands
{
    public class ChangePrescriptionStatus:IRequest<Result>
    {
        public Guid OrderId { get; }
        public OrderStatus Status { get; }

        public ChangePrescriptionStatus(Guid orderId, OrderStatus status)
        {
            OrderId = orderId;
            Status = status;
        }
    }

    public class ChangePrescriptionStatusHandler : IRequestHandler<ChangePrescriptionStatus, Result>
    {
        private readonly IMediator _mediator;
        private readonly IPrescriptionRepository _prescriptionRepository;

        public ChangePrescriptionStatusHandler(IMediator mediator, IPrescriptionRepository prescriptionRepository)
        {
            _mediator = mediator;
            _prescriptionRepository = prescriptionRepository;
        }

        public async Task<Result> Handle(ChangePrescriptionStatus request, CancellationToken cancellationToken)
        {
            try
            {
                var order = await _prescriptionRepository.GetAsync(request.OrderId);

                if (null == order)
                    throw new Exception("Order not found");

                order.ChangeStatus(request.Status);

                await _prescriptionRepository.CreateOrUpdateAsync(order);

               return Result.Success();
            }
            catch (Exception e)
            {
                var msg = $"Error {request.GetType().Name}";
                Log.Error(e, msg);
                return Result.Failure(msg);
            }
        }
    }
}
