using System;
using System.ComponentModel.DataAnnotations.Schema;
using LiveClinic.SharedKernel.Domain;

namespace LiveClinic.Pharmacy.Core.Domain.Orders
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
        [NotMapped]
        public bool IsStocked { get; set; }

        [NotMapped] public bool IsValidated =>null!=DrugId && DrugId.HasValue && DrugId != Guid.Empty;

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
