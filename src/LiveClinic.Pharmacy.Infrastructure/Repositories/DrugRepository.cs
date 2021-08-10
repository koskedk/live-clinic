using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LiveClinic.Pharmacy.Core.Domain.DrugAggregate;
using LiveClinic.SharedKernel.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LiveClinic.Pharmacy.Infrastructure.Repositories
{
    public class DrugRepository:BaseRepository<Drug,Guid>, IDrugRepository
    {
        public DrugRepository(PharmacyDbContext context) : base(context)
        {
        }

        public List<Drug> LoadAll(Expression<Func<Drug, bool>> predicate = null)
        {
            if(null==predicate)
                return GetAll().Include(x => x.Transactions)
                    .ToList();

            return GetAll(predicate).Include(x => x.Transactions)
                .ToList();
        }
    }
}
