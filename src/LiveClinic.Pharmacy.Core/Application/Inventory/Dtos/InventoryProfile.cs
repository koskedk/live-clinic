using AutoMapper;
using LiveClinic.Pharmacy.Core.Domain.Inventory;

namespace LiveClinic.Pharmacy.Core.Application.Inventory.Dtos
{
    public class InventoryProfile : Profile
    {
        public InventoryProfile()
        {
            CreateMap<StockTransaction, StockTransactionDto>();
            CreateMap<Drug, InventoryDto>();
        }
    }
}
