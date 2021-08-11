using AutoMapper;
using LiveClinic.Contracts;
using LiveClinic.Pharmacy.Core.Domain.Orders;
using OrderItem = LiveClinic.Contracts.OrderItem;

namespace LiveClinic.Pharmacy.Core.Application.Orders.Dtos
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            //Contracts
            CreateMap<OrderItem, PrescriptionOrderItem>();
            CreateMap<OrderGenerated, PrescriptionOrder>();

            // out
            CreateMap<PrescriptionOrderItem, OrderItem>();
            CreateMap<PrescriptionOrder, OrderAccepted>();
            CreateMap<PrescriptionOrder, OrderRejected>();
        }
    }
}
