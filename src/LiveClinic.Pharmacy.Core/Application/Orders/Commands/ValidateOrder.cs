using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using LiveClinic.Pharmacy.Core.Domain.Inventory;
using LiveClinic.Pharmacy.Core.Domain.Orders;
using LiveClinic.Pharmacy.Core.Domain.Orders.Events;
using MediatR;
using Serilog;

namespace LiveClinic.Pharmacy.Core.Application.Orders.Commands
{
    public class ValidateOrder : IRequest<Result>
    {
        public PrescriptionOrder Order { get; }
        public ValidateOrder(PrescriptionOrder order)
        {
            Order = order;
        }
    }

    public class ValidateOrderHandler : IRequestHandler<ValidateOrder, Result>
    {
        private readonly IMediator _mediator;
        private readonly IDrugRepository _drugRepository;
        private readonly IPrescriptionOrderRepository _prescriptionOrderRepository;
        private readonly IMapper _mapper;

        public ValidateOrderHandler(IMediator mediator, IDrugRepository drugRepository, IMapper mapper, IPrescriptionOrderRepository prescriptionOrderRepository)
        {
            _mediator = mediator;
            _drugRepository = drugRepository;
            _mapper = mapper;
            _prescriptionOrderRepository = prescriptionOrderRepository;
        }

        public async Task<Result> Handle(ValidateOrder request, CancellationToken cancellationToken)
        {
             try
            {
                if (request.Order.HasNoAssignedIds())
                    request.Order.AssignIds();

                foreach (var d in request.Order.OrderItems)
                {
                    var drug =  _drugRepository.LoadAll(x => x.Code == d.DrugCode).FirstOrDefault();
                    if (null == drug)
                        throw new Exception("Drug NOT Found!");
                    d.IsStocked = drug.IsStocked(d.QuantityPrescribed);
                    d.DrugId = drug.Id;
                }

                // save order
                await _prescriptionOrderRepository.CreateOrUpdateAsync(request.Order);

                var orderValidated = new OrderValidated(request.Order.Id, request.Order.AllInStock);
                await _mediator.Publish(orderValidated);

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
