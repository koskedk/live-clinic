using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LiveClinic.SharedKernel.Domain.Repositories;

namespace LiveClinic.Pharmacy.Core.Domain.Orders
{
    public interface IPrescriptionOrderRepository : IRepository<PrescriptionOrder, Guid>
    {
        Task<PrescriptionOrder> GetByOrder(Guid orderId);
        List<PrescriptionOrder> LoadAll(Expression<Func<PrescriptionOrder, bool>> predicate = null);
    }
}
