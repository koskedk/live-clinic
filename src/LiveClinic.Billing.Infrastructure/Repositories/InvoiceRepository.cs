using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LiveClinic.Billing.Core.Domain.InvoiceAggregate;
using LiveClinic.Billing.Infrastructure.Persistence;
using LiveClinic.SharedKernel.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LiveClinic.Billing.Infrastructure.Repositories
{
    public class InvoiceRepository:BaseRepository<Invoice,Guid>, IInvoiceRepository
    {
        public InvoiceRepository(BillingDbContext context) : base(context)
        {
        }

        public Invoice UpdatePayments(Guid id, Payment payment)
        {
            var ctx = Context as BillingDbContext;

            var inv = ctx.Invoices.Include(x => x.Items)
                .Include(x => x.Payments)
                .SingleOrDefault(x => x.Id == id);
            inv.MakePayment(payment);
            ctx.Entry(inv).State = EntityState.Modified;
            inv.Items.ForEach(x => ctx.Entry(x).State = EntityState.Unchanged);
            ctx.Entry(payment).State = EntityState.Added;
            ctx.Invoices.Update(inv);
            ctx.SaveChanges();

            return inv;
        }

        public List<Invoice> LoadAll(Expression<Func<Invoice, bool>> predicate = null)
        {
            if(null==predicate)
                return GetAll()
                    .Include(x => x.Items).ThenInclude(p=>p.PriceCatalog)
                    .Include(x=>x.Payments)
                    .ToList();

            return GetAll(predicate)
                .Include(x => x.Items).ThenInclude(p=>p.PriceCatalog)
                .Include(x=>x.Payments)
                .ToList();
        }
    }
}
