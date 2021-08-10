using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LiveClinic.SharedKernel.Domain.Repositories;

namespace LiveClinic.Billing.Core.Domain.InvoiceAggregate
{
    public interface IInvoiceRepository : IRepository<Invoice, Guid>
    {
        List<Invoice> LoadAll(Expression<Func<Invoice, bool>> predicate = null);
    }
}
