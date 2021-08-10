using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LiveClinic.SharedKernel.Domain.Repositories;

namespace LiveClinic.Pharmacy.Core.Domain.DrugAggregate
{
    public interface IDrugRepository : IRepository<Drug, Guid>
    {
        List<Drug> LoadAll(Expression<Func<Drug, bool>> predicate = null);
    }
}
