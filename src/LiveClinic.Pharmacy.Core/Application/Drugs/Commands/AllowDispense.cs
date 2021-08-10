using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using LiveClinic.Pharmacy.Core.Domain.DrugAggregate;
using LiveClinic.Pharmacy.Core.Domain.PrescriptionOrderAggregate;
using MediatR;
using Serilog;

namespace LiveClinic.Pharmacy.Core.Application.Commands
{
    public class AllowDispense : IRequest<Result>
    {
        public  Guid OrderId { get;  }
        public Guid InvoiceId { get;   }

        public AllowDispense(Guid orderId, Guid invoiceId)
        {
            OrderId = orderId;
            InvoiceId = invoiceId;
        }
    }

    public class AllowDispenseHandler : IRequestHandler<AllowDispense, Result>
    {
        private readonly IMediator _mediator;
        private readonly IPrescriptionOrderRepository _prescriptionOrderRepository;

        public AllowDispenseHandler(IMediator mediator, IPrescriptionOrderRepository drugRepository)
        {
            _mediator = mediator;
            _prescriptionOrderRepository = drugRepository;
        }

        public async Task<Result> Handle(AllowDispense request, CancellationToken cancellationToken)
        {
            try
            {
                var order = _prescriptionOrderRepository.GetAll(x => x.OrderId == request.OrderId).FirstOrDefault();

                if (null == order)
                    throw new Exception("Order not found");

                order.SetPaymentInfo(request.InvoiceId);

                await _prescriptionOrderRepository.CreateOrUpdateAsync(order);

                return Result.Success();
            }
            catch (Exception e)
            {
                var msg = $"Error {request.GetType().Name}";
                Log.Error(msg, e);
                return Result.Failure(msg);
            }
        }
    }
}
