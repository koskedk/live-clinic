using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using LiveClinic.Pharmacy.Core.Application.Orders.Dtos;
using LiveClinic.Pharmacy.Core.Domain.Orders;
using MediatR;
using Serilog;

namespace LiveClinic.Pharmacy.Core.Application.Orders.Queries
{
    public class GetActiveOrders : IRequest<Result<List<PrescriptionOrder>>>
    {
        public Guid? OrderId { get; }

        public GetActiveOrders(Guid? orderId = null)
        {
            OrderId = orderId;
        }
    }

    public class GetActiveOrdersHandler : IRequestHandler<GetActiveOrders, Result<List<PrescriptionOrder>>>
    {
        private readonly IPrescriptionOrderRepository _prescriptionOrderRepository;

        public GetActiveOrdersHandler(IPrescriptionOrderRepository prescriptionOrderRepository)
        {
            _prescriptionOrderRepository = prescriptionOrderRepository;
        }

        public Task<Result<List<PrescriptionOrder>>> Handle(GetActiveOrders request, CancellationToken cancellationToken)
        {
            try
            {
                var orders = new List<PrescriptionOrder>();

                if (request.OrderId.HasValue)
                {
                    orders = _prescriptionOrderRepository
                        .LoadAll(x =>
                            x.OrderId == request.OrderId.Value &&
                            x.Status == PrescriptionStatus.Active)
                        .ToList();
                }
                else
                {
                    orders = _prescriptionOrderRepository
                        .LoadAll(x => x.Status == PrescriptionStatus.Active)
                        .ToList();
                }

                return Task.FromResult(Result.Success(orders));
            }
            catch (Exception e)
            {
                var msg = $"Error {request.GetType().Name}";
                Log.Error(e, msg);
                return Task.FromResult(Result.Failure<List<PrescriptionOrder>>(msg));
            }
        }
    }
}
