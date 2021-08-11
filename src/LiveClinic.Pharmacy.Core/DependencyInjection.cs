using System.Collections.Generic;
using System.Reflection;
using LiveClinic.Pharmacy.Core.Application.Inventory.Commands;
using LiveClinic.Pharmacy.Core.Application.Inventory.Dtos;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace LiveClinic.Pharmacy.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCore(this IServiceCollection services, List<Assembly> mediatrAssemblies = null)
        {
            services.AddAutoMapper(typeof(PharmacyProfile));

            if (null != mediatrAssemblies)
            {
                mediatrAssemblies.Add(typeof(DispenseDrugsHandler).Assembly);
                services.AddMediatR(mediatrAssemblies.ToArray());
            }
            else
            {
                services.AddMediatR(typeof(DispenseDrugsHandler).Assembly);
            }

            return services;
        }
    }
}
