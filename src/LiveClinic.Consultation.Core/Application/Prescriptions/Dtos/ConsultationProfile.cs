using AutoMapper;
using LiveClinic.Consultation.Core.Domain.Prescriptions;
using LiveClinic.Contracts;
using Medication = LiveClinic.Consultation.Core.Domain.Prescriptions.Medication;

namespace LiveClinic.Consultation.Core.Application.Prescriptions.Dtos
{
    public class ConsultationProfile : Profile
    {
        public ConsultationProfile()
        {
            //  Contracts
            CreateMap<Medication, OrderItem>();
            CreateMap<Prescription, OrderGenerated>()
                .ForMember(dest => dest.OrderId,  opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.OrderItems,  opt => opt.MapFrom(src => src.Medications));
        }
    }
}
