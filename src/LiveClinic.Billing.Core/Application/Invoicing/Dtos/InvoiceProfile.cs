using AutoMapper;
using LiveClinic.Billing.Core.Domain.InvoiceAggregate;
using LiveClinic.Contracts;

namespace LiveClinic.Billing.Core.Application.Invoicing.Dtos
{
    public class InvoiceProfile : Profile
    {
        public InvoiceProfile()
        {
            CreateMap<Payment, InvoicePaymentDto>();
            CreateMap<InvoiceItem, InvoiceLineDto>()
                .ForMember(dest => dest.Item, opt => opt.MapFrom(src => src.PriceCatalog.Name))
                .ForMember(dest => dest.DrugCode, opt => opt.MapFrom(src => src.PriceCatalog.DrugCode));

            CreateMap<Invoice, InvoiceSummaryDto>();
            CreateMap<Invoice, InvoiceDto>();
            CreateMap<OrderItem, OrderInvoiceItemDto>();
            CreateMap<DrugOrderValidated, OrderInvoiceDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Medications));

            CreateMap<InvoiceItem, InvoiceItemDto>()
                .ForMember(dest => dest.DrugCode, opt => opt.MapFrom(src => src.PriceCatalog.DrugCode));

            CreateMap<InvoiceItemDto, OrderItem>();

            CreateMap<InvoiceDto, DrugOrderPaid>();
        }
    }
}
