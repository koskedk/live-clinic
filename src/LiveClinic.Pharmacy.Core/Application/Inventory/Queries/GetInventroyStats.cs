using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using LiveClinic.Pharmacy.Core.Application.Inventory.Dtos;
using LiveClinic.Pharmacy.Core.Domain.Inventory;
using MediatR;
using Serilog;

namespace LiveClinic.Pharmacy.Core.Application.Inventory.Queries
{
    public class GetInventoryStats : IRequest<Result<InventoryStatsDto>>
    {
        public Guid? DrugId { get; }

        public GetInventoryStats(Guid? drugId = null)
        {
            DrugId = drugId;
        }
    }

    public class GetInventoryStatsHandler : IRequestHandler<GetInventoryStats,Result<InventoryStatsDto>>
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IDrugRepository _drugRepository;


        public GetInventoryStatsHandler( IMapper mapper,IDrugRepository drugRepository, IMediator mediator)
        {
            _mapper = mapper;
            _drugRepository = drugRepository;
            _mediator = mediator;
        }

        public async Task<Result<InventoryStatsDto>> Handle(GetInventoryStats request, CancellationToken cancellationToken)
        {
            try
            {
                var res =await _mediator.Send(new GetInventory());
                if (res.IsFailure)
                    throw new Exception(res.Error);

                var stats = InventoryStatsDto.Generate(res.Value);

                return Result.Success(stats);
            }
            catch (Exception e)
            {
                var msg = $"Error {request.GetType().Name}";
                Log.Error(e, msg);
                return Result.Failure<InventoryStatsDto>(msg);
            }
        }
    }
}
