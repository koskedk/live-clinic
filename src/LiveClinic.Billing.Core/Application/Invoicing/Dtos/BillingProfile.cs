using AutoMapper;
using LiveClinic.Billing.Core.Domain.InvoiceAggregate;
using LiveClinic.Contracts;

namespace LiveClinic.Billing.Core.Application.Invoicing.Dtos
{
    public class BillingProfile : Profile
    {
        public BillingProfile()
        {
            //Contracts
            CreateMap<OrderItem, OrderInvoiceItemDto>();
            CreateMap<OrderAccepted, OrderInvoiceDto>();

            ///////////////////

            //Invoice-Summary
            CreateMap<Payment, InvoicePaymentDto>();
            CreateMap<InvoiceItem, InvoiceLineDto>()
                .ForMember(dest => dest.Item, opt => opt.MapFrom(src => src.PriceCatalog.Name))
                .ForMember(dest => dest.DrugCode, opt => opt.MapFrom(src => src.PriceCatalog.DrugCode));
            CreateMap<Invoice, InvoiceSummaryDto>();

            // factory
            CreateMap<Invoice, InvoiceDto>();
            CreateMap<InvoiceItem, InvoiceItemDto>()
                .ForMember(dest => dest.DrugCode, opt => opt.MapFrom(src => src.PriceCatalog.DrugCode));
        }
    }
}
