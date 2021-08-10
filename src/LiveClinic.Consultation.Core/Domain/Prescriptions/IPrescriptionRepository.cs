using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LiveClinic.SharedKernel.Domain.Repositories;

namespace LiveClinic.Consultation.Core.Domain.Prescriptions
{
    public interface IPrescriptionRepository : IRepository<Prescription, Guid>
    {
        List<Prescription> LoadAll(Expression<Func<Prescription, bool>> predicate = null);
    }
}
