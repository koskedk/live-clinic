using System.Collections.Generic;
using System.Reflection;
using LiveClinic.Consultation.Core.Application.Prescriptions.Commands;
using LiveClinic.Consultation.Core.Application.Prescriptions.Dtos;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace LiveClinic.Consultation.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCore(this IServiceCollection services, List<Assembly> mediatrAssemblies = null)
        {
             services.AddAutoMapper(typeof(ConsultationProfile));

            if (null != mediatrAssemblies)
            {
                mediatrAssemblies.Add(typeof(PrescribeDrugsHandler).Assembly);
                services.AddMediatR(mediatrAssemblies.ToArray());
            }
            else
            {
                services.AddMediatR(typeof(PrescribeDrugsHandler).Assembly);
            }

            return services;
        }
    }
}
