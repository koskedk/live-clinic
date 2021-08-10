using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LiveClinic.Consultation.Core.Domain.Prescriptions;
using LiveClinic.SharedKernel.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LiveClinic.Consultation.Infrastructure.Repositories
{
    public class PrescriptionRepository:BaseRepository<Prescription,Guid>, IPrescriptionRepository
    {
        public PrescriptionRepository(ConsultationDbContext context) : base(context)
        {
        }

        public List<Prescription> LoadAll(Expression<Func<Prescription, bool>> predicate = null)
        {
            if(null==predicate)
                return GetAll().Include(x => x.Medications)
                    .ToList();

            return GetAll(predicate).Include(x => x.Medications)
                .ToList();
        }
    }
}
