using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LiveClinic.SharedKernel.Domain.Repositories;

namespace LiveClinic.Billing.Core.Domain.PriceAggregate
{
    public interface IPriceCatalogRepository : IRepository<PriceCatalog, Guid>
    {
        Task<PriceCatalog> GetPrice(Expression<Func<PriceCatalog, bool>> predicate);
    }
}
