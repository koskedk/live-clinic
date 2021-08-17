using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using LiveClinic.Pharmacy.Core.Application.Inventory.Dtos;
using LiveClinic.Pharmacy.Core.Domain.Inventory;
using LiveClinic.Pharmacy.Core.Domain.Inventory.Events;
using MediatR;
using Serilog;

namespace LiveClinic.Pharmacy.Core.Application.Inventory.Commands
{
    public class ReceiveStock : IRequest<Result>
    {
        public List<DrugReceiptDto> Stocks { get; }

        public ReceiveStock(List<DrugReceiptDto> stocks)
        {
            Stocks = stocks;
        }
    }

    public class ReceiveStockHandler : IRequestHandler<ReceiveStock, Result>
    {
        private readonly IMediator _mediator;
        private readonly IDrugRepository _drugRepository;

        public ReceiveStockHandler(IMediator mediator, IDrugRepository drugRepository)
        {
            _mediator = mediator;
            _drugRepository = drugRepository;
        }

        public async Task<Result> Handle(ReceiveStock request, CancellationToken cancellationToken)
        {
            try
            {
                foreach (var stock in request.Stocks)
                {
                    var drug = await  _drugRepository.GetAsync(stock.DrugId);
                    if (null == drug)
                        throw new Exception("Drug NOT Found!");

                    var newStock= drug.ReceiveStock(stock.BatchNo,stock.Quantity,stock.OrderRef);
                    await _drugRepository.CreateOrUpdateAsync<StockTransaction,Guid>(new[] {newStock});

                    await _mediator.Publish(new StockReceived(newStock.Id), cancellationToken);
                }

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
