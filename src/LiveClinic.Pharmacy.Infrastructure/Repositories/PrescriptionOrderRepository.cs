using System;
using LiveClinic.Pharmacy.Core.Domain;
using LiveClinic.Pharmacy.Core.Domain.DrugAggregate;
using LiveClinic.Pharmacy.Core.Domain.PrescriptionOrderAggregate;
using LiveClinic.SharedKernel.Infrastructure.Repositories;

namespace LiveClinic.Pharmacy.Infrastructure.Repositories
{
    public class PrescriptionOrderRepository:BaseRepository<PrescriptionOrder,Guid>, IPrescriptionOrderRepository
    {
        public PrescriptionOrderRepository(PharmacyDbContext context) : base(context)
        {
        }  
    }
}