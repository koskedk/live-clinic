using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LiveClinic.Pharmacy.Core.Domain.Orders;
using LiveClinic.SharedKernel.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LiveClinic.Pharmacy.Infrastructure.Repositories
{
    public class PrescriptionOrderRepository:BaseRepository<PrescriptionOrder,Guid>, IPrescriptionOrderRepository
    {
        public PrescriptionOrderRepository(PharmacyDbContext context) : base(context)
        {
        }

        public Task<PrescriptionOrder> GetByOrder(Guid orderId)
        {
            return GetAllTracked(x => x.OrderId == orderId).FirstOrDefaultAsync();
        }

        public List<PrescriptionOrder> LoadAll(Expression<Func<PrescriptionOrder, bool>> predicate = null)
        {
            if(null==predicate)
                return GetAll().Include(x => x.OrderItems)
                    .ToList();

            return GetAll(predicate).Include(x => x.OrderItems)
                .ToList();
        }
    }
}
