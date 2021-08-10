using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LiveClinic.Billing.Core.Domain.PriceAggregate;
using LiveClinic.Billing.Infrastructure.Persistence;
using LiveClinic.SharedKernel.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LiveClinic.Billing.Infrastructure.Repositories
{
    public class PriceCatalogRepository:BaseRepository<PriceCatalog,Guid>, IPriceCatalogRepository
    {
        public PriceCatalogRepository(BillingDbContext context) : base(context)
        {
        }

        public Task<PriceCatalog> GetPrice(Expression<Func<PriceCatalog, bool>> predicate)
        {
           return GetAll(predicate).FirstOrDefaultAsync();
        }
    }
}
