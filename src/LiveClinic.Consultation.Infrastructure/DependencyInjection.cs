using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LiveClinic.Consultation.Core.Domain.Prescriptions;
using LiveClinic.Consultation.Infrastructure.Repositories;
using LiveClinic.SharedKernel.Config;
using MassTransit;
using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LiveClinic.Consultation.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services,
            IConfiguration configuration, bool initDb = true)
        {
            if (initDb)
                services.AddDbContext<ConsultationDbContext>(o => o.UseSqlServer(
                    configuration.GetConnectionString("DatabaseConnection"),
                    x =>
                        x.MigrationsAssembly(typeof(ConsultationDbContext).Assembly.FullName)
                ));

            services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
            return services;
        }

        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration,
            bool initBus = true)
        {
            if (initBus)
            {
                var positionOptions = new RabbitMqOptions();
                configuration.GetSection(RabbitMqOptions.RabbitMq).Bind(positionOptions);

                services.AddMassTransit(mt =>
                {
                    mt.SetKebabCaseEndpointNameFormatter();
                    mt.UsingRabbitMq((context, cfg) =>
                    {
                        cfg.Host(positionOptions.Host, positionOptions.VirtualHost, h =>
                            {
                                h.Username(positionOptions.Username);
                                h.Password(positionOptions.Password);
                            }
                        );
                        cfg.ConfigureEndpoints(context);
                    });
                });
            }
            else
            {
                services.AddMassTransitInMemoryTestHarness();
            }
            return services;
        }
    }
}
