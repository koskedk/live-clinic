using LiveClinic.Consultation.Core.Domain.Prescriptions;
using LiveClinic.SharedKernel.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LiveClinic.Consultation.Infrastructure
{
    public class ConsultationDbContext:BaseContext
    {
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Medication> Medications { get; set; }

        public ConsultationDbContext(DbContextOptions<ConsultationDbContext> options) : base(options)
        {
        }
    }
}
