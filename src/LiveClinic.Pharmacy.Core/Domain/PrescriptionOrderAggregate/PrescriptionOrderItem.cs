using System;
using System.ComponentModel.DataAnnotations.Schema;
using LiveClinic.SharedKernel.Domain;

namespace LiveClinic.Pharmacy.Core.Domain.PrescriptionOrderAggregate
{
    public class PrescriptionOrderItem:Entity<Guid>
    {
        public string DrugCode { get; set; }
        public double Days { get; set; }
        public double Quantity { get; set; }
        public double QuantityDispensed { get; set; }
        public Guid? DrugId { get; set; }
        public Guid PrescriptionOrderId { get; set; }

        [NotMapped]
        public double QuantityPrescribed => Quantity * Days;

        public void UpdateDispense(int dispensed)
        {
            QuantityDispensed = dispensed;
        }

        public void UpdateDrugId(Guid drugId)
        {
            DrugId = drugId;
        }
    }
}
