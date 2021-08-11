using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using LiveClinic.Pharmacy.Core.Domain.Orders;
using LiveClinic.Pharmacy.Core.Domain.Orders.Events;
using MediatR;
using Serilog;

namespace LiveClinic.Pharmacy.Core.Application.Orders.Commands
{
    public class ReserveOrder : IRequest<Result>
    {
        public  Guid OrderId { get;  }
        public Guid PaymentId { get;   }

        public ReserveOrder(Guid orderId, Guid paymentId)
        {
            OrderId = orderId;
            PaymentId = paymentId;
        }
    }

    public class ReserveOrderHandler : IRequestHandler<ReserveOrder, Result>
    {
        private readonly IMediator _mediator;
        private readonly IPrescriptionOrderRepository _prescriptionOrderRepository;

        public ReserveOrderHandler(IMediator mediator, IPrescriptionOrderRepository drugRepository)
        {
            _mediator = mediator;
            _prescriptionOrderRepository = drugRepository;
        }

        public async Task<Result> Handle(ReserveOrder request, CancellationToken cancellationToken)
        {
            try
            {
                var order =await  _prescriptionOrderRepository.GetByOrder(request.OrderId);

                if (null == order)
                    throw new Exception("Order not found");

                order.SetPaymentInfo(request.PaymentId);

                await _prescriptionOrderRepository.CreateOrUpdateAsync(order);

                await _mediator.Publish(new OrderReserved(request.OrderId),cancellationToken);

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
