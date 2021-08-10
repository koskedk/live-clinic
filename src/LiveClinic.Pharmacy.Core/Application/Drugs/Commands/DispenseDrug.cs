using System;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using LiveClinic.Pharmacy.Core.Application.Dtos;
using LiveClinic.Pharmacy.Core.Domain.DrugAggregate;
using MediatR;
using Serilog;

namespace LiveClinic.Pharmacy.Core.Application.Commands
{
    public class DispenseDrug : IRequest<Result>
    {
        public StockOutDto StockOutDto { get; }
        public DispenseDrug(StockOutDto stockOutDto)
        {
            StockOutDto = stockOutDto;
        }
    }

    public class DispenseDrugHandler : IRequestHandler<DispenseDrug, Result>
    {
        private readonly IMediator _mediator;
        private readonly IDrugRepository _drugRepository;

        public DispenseDrugHandler(IMediator mediator, IDrugRepository drugRepository)
        {
            _mediator = mediator;
            _drugRepository = drugRepository;
        }

        public async Task<Result> Handle(DispenseDrug request, CancellationToken cancellationToken)
        {
            try
            {
                foreach (var d in request.StockOutDto.Medications)
                {
                    var drug = await  _drugRepository.GetAsync(d.DrugId);
                    if (null == drug)
                        throw new Exception("Drug NOT Found!");

                    // var dispenseStock = drug.Dispense(request.BatchNo,request.Quantity,request.OrderRef);
                   //  await _drugRepository.CreateOrUpdateAsync<StockTransaction,Guid>(new[] {dispenseStock});
                }
               // await _mediator.Publish(new DrugDispensed(dispenseStock.Id), cancellationToken);
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
