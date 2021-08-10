using System;
using LiveClinic.SharedKernel.Domain;

namespace LiveClinic.Consultation.Core.Domain.Prescriptions
{
    public class Medication:Entity<Guid>
    {
        public string DrugCode { get; private set; }
        public double Days { get;  private set;}
        public double Quantity { get; private set;}
        public DateTime Generated { get;private set; }
        public Guid PrescriptionId { get; private set;}

        public Medication(string drugCode, double days, double quantity, Guid prescriptionId)
        {
            DrugCode = drugCode;
            Days = days;
            Quantity = quantity;
            PrescriptionId = prescriptionId;
            Generated = DateTime.Now;
        }

        public override string ToString()
        {
            return $"{DrugCode} {Quantity}>{Days} days";
        }
    }
}
