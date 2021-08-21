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
    public class GetOrders : IRequest<Result<List<PrescriptionOrder>>>
    {
        public Guid? OrderId { get; }
        public PrescriptionStatus Status { get; }

        public GetOrders(PrescriptionStatus status, Guid? orderId = null)
        {
            Status = status;
            OrderId = orderId;
        }
    }

    public class GetOrdersHandler : IRequestHandler<GetOrders, Result<List<PrescriptionOrder>>>
    {
        private readonly IPrescriptionOrderRepository _prescriptionOrderRepository;

        public GetOrdersHandler(IPrescriptionOrderRepository prescriptionOrderRepository)
        {
            _prescriptionOrderRepository = prescriptionOrderRepository;
        }

        public Task<Result<List<PrescriptionOrder>>> Handle(GetOrders request, CancellationToken cancellationToken)
        {
            try
            {
                var orders = new List<PrescriptionOrder>();

                if (request.OrderId.HasValue)
                {
                    orders = _prescriptionOrderRepository
                        .LoadAll(x =>
                            x.OrderId == request.OrderId.Value &&
                            x.Status == request.Status)
                        .ToList();
                }
                else
                {
                    orders = _prescriptionOrderRepository
                        .LoadAll(x => x.Status == request.Status)
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
