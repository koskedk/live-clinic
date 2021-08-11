using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using LiveClinic.Pharmacy.Core.Domain.Inventory;
using LiveClinic.Pharmacy.Core.Domain.Inventory.Events;
using LiveClinic.Pharmacy.Core.Domain.Orders;
using MediatR;
using Serilog;

namespace LiveClinic.Pharmacy.Core.Application.Inventory.Commands
{
    public class DispenseDrugs : IRequest<Result>
    {
        public Guid OrderId { get; }

        public DispenseDrugs(Guid orderId)
        {
            OrderId = orderId;
        }
    }

    public class DispenseDrugsHandler : IRequestHandler<DispenseDrugs, Result>
    {
        private readonly IMediator _mediator;
        private readonly IDrugRepository _drugRepository;
        private readonly IPrescriptionOrderRepository _orderRepository;

        public DispenseDrugsHandler(IMediator mediator, IDrugRepository drugRepository, IPrescriptionOrderRepository orderRepository)
        {
            _mediator = mediator;
            _drugRepository = drugRepository;
            _orderRepository = orderRepository;
        }

        public async Task<Result> Handle(DispenseDrugs request, CancellationToken cancellationToken)
        {
            try
            {
                var order = _orderRepository.LoadAll(x => x.OrderId == request.OrderId)
                    .FirstOrDefault();

                if (null == order)
                    throw new Exception("No order found");

                if(!order.IsReserved)
                    throw new Exception("order NOT paid");

                foreach (var d in order.OrderItems.Where(x=>x.IsValidated))
                {
                    var drug = await  _drugRepository.GetAsync(d.DrugId.Value);
                    if (null == drug)
                        throw new Exception("Drug NOT Found!");

                    var dispenseStock = drug.Dispense(d.Quantity, order.OrderId.ToString());

                    await _drugRepository.CreateOrUpdateAsync<StockTransaction,Guid>(new[] {dispenseStock});
                }

                await _mediator.Publish(new DrugsDispensed(request.OrderId), cancellationToken);

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
