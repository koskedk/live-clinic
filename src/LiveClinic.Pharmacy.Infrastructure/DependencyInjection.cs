using LiveClinic.Pharmacy.Core.Application.EventHandlers;
using LiveClinic.Pharmacy.Core.Domain.DrugAggregate;
using LiveClinic.Pharmacy.Core.Domain.PrescriptionOrderAggregate;
using LiveClinic.Pharmacy.Infrastructure.Repositories;
using LiveClinic.SharedKernel.Config;
using MassTransit;
using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LiveClinic.Pharmacy.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services,
            IConfiguration configuration, bool initDb = true)
        {
            if (initDb)
                services.AddDbContext<PharmacyDbContext>(o => o.UseSqlServer(
                    configuration.GetConnectionString("DatabaseConnection"),
                    x =>  x.MigrationsAssembly(typeof(PharmacyDbContext).Assembly.FullName)
                    ));

            services.AddScoped<IDrugRepository, DrugRepository>();
            services.AddScoped<IPrescriptionOrderRepository, PrescriptionOrderRepository>();
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
                    mt.AddConsumersFromNamespaceContaining<OrderGeneratedHandler>();
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
                services.AddMassTransitInMemoryTestHarness(mt =>
                {
                    mt.AddConsumersFromNamespaceContaining<OrderGeneratedHandler>();
                });
            }
            return services;
        }
    }
}
