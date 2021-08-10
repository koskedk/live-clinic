using System;
using LiveClinic.SharedKernel.Domain.Repositories;

namespace LiveClinic.Pharmacy.Core.Domain.PrescriptionOrderAggregate
{
    public interface IPrescriptionOrderRepository : IRepository<PrescriptionOrder, Guid>
    {
    }
}
