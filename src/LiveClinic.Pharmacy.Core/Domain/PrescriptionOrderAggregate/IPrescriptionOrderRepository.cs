using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LiveClinic.Pharmacy.Core.Domain.DrugAggregate;
using LiveClinic.SharedKernel.Domain.Repositories;

namespace LiveClinic.Pharmacy.Core.Domain.PrescriptionOrderAggregate
{
    public interface IPrescriptionOrderRepository : IRepository<PrescriptionOrder, Guid>
    {
        List<PrescriptionOrder> LoadAll(Expression<Func<PrescriptionOrder, bool>> predicate = null);
    }
}
