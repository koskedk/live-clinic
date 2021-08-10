using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using LiveClinic.Pharmacy.Core.Application.Dtos;
using LiveClinic.Pharmacy.Core.Domain.DrugAggregate;
using MediatR;
using Serilog;

namespace LiveClinic.Pharmacy.Core.Application.Queries
{
    public class GetInventory : IRequest<Result<List<InventoryDto>>>
    {
        public Guid? DrugId { get; }

        public GetInventory(Guid? drugId = null)
        {
            DrugId = drugId;
        }
    }

    public class GetInventoryHandler : IRequestHandler<GetInventory,Result<List<InventoryDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IDrugRepository _drugRepository;


        public GetInventoryHandler( IMapper mapper,IDrugRepository drugRepository)
        {
            _mapper = mapper;
            _drugRepository = drugRepository;
        }

        public Task<Result<List<InventoryDto>>> Handle(GetInventory request, CancellationToken cancellationToken)
        {
            try
            {
                var drugs=new List<Drug>();

                if(request.DrugId.HasValue)
                    drugs =  _drugRepository.LoadAll(x=>x.Id==request.DrugId.Value).ToList();
                else
                    drugs =  _drugRepository.LoadAll().ToList();

                var inventoryDtos = _mapper.Map<List<InventoryDto>>(drugs);

                return Task.FromResult(Result.Success(inventoryDtos));
            }
            catch (Exception e)
            {
                var msg = $"Error {request.GetType().Name}";
                Log.Error(msg, e);
                return Task.FromResult(Result.Failure<List<InventoryDto>>(msg));
            }
        }
    }
}
