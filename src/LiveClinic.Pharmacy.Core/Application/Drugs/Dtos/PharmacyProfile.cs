using AutoMapper;
using LiveClinic.Contracts;
using LiveClinic.Pharmacy.Core.Domain;
using LiveClinic.Pharmacy.Core.Domain.DrugAggregate;
using LiveClinic.Pharmacy.Core.Domain.PrescriptionOrderAggregate;
using OrderItem = LiveClinic.Contracts.OrderItem;

namespace LiveClinic.Pharmacy.Core.Application.Dtos
{
    public class PharmacyProfile : Profile
    {
        public PharmacyProfile()
        {
            //Contracts in
            CreateMap<OrderItem, PrescriptionOrderItem>();
            CreateMap<OrderGenerated, PrescriptionOrder>();
            // out
            CreateMap<PrescriptionOrderItem, OrderItem>();
            CreateMap<PrescriptionOrder, OrderAccepted>();
            CreateMap<PrescriptionOrder, OrderRejected>();

            ///////////////

            CreateMap<StockTransaction, StockTransactionDto>();
            CreateMap<Drug, InventoryDto>();
            CreateMap<OrderGenerated, OrderAccepted>();

            CreateMap<DrugOrderPaid, StockOutDto>();
            CreateMap<OrderItem, DrugDto>();
        }
    }
}
