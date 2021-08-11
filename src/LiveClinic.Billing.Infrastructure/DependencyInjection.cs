using System;
using LiveClinic.Billing.Core.Application.Invoicing.IntegrationEventHandlers;
using LiveClinic.Billing.Core.Domain.InvoiceAggregate;
using LiveClinic.Billing.Core.Domain.PriceAggregate;
using LiveClinic.Billing.Infrastructure.Persistence;
using LiveClinic.Billing.Infrastructure.Repositories;
using LiveClinic.SharedKernel.Config;
using MassTransit;
using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LiveClinic.Billing.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services,
            IConfiguration configuration, bool initDb = true)
        {
            if (initDb)
                services.AddDbContext<BillingDbContext>(o => o.UseSqlServer(
                    configuration.GetConnectionString("DatabaseConnection"),
                    x => x.MigrationsAssembly(typeof(BillingDbContext).Assembly.FullName)
                ));

            services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            services.AddScoped<IPriceCatalogRepository, PriceCatalogRepository>();
            return services;
        }

        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration,
            bool initBus = true,Type consumerType=null)
        {
            if (initBus)
            {
                var positionOptions = new RabbitMqOptions();
                configuration.GetSection(RabbitMqOptions.RabbitMq).Bind(positionOptions);

                services.AddMassTransit(mt =>
                {
                    mt.SetKebabCaseEndpointNameFormatter();
                    mt.AddConsumersFromNamespaceContaining<OrderAcceptedHandler>();
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
                    mt.AddConsumersFromNamespaceContaining<OrderAcceptedHandler>();
                    if(null!=consumerType)
                        mt.AddConsumersFromNamespaceContaining(consumerType);
                });
            }
            return services;
        }
    }
}
