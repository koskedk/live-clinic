using LiveClinic.Billing.Core.Domain.InvoiceAggregate;
using LiveClinic.Billing.Core.Domain.PriceAggregate;
using LiveClinic.Billing.Infrastructure.Persistence;
using LiveClinic.Billing.Infrastructure.Repositories;
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
    }
}
