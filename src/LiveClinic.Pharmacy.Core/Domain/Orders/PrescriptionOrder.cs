using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using LiveClinic.SharedKernel.Domain;

namespace LiveClinic.Pharmacy.Core.Domain.Orders
{
    public class PrescriptionOrder:AggregateRoot<Guid>
    {
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderNo { get;set;   }
        public string Patient { get; set; }
        public string Provider { get; set; }
        public Guid? PaymentId { get; set; }
        public PrescriptionStatus Status { get; set; }
        public List<PrescriptionOrderItem> OrderItems { get; set; } = new List<PrescriptionOrderItem>();
        [NotMapped] public bool AllInStock =>  OrderItems.All(x => x.IsStocked);
        [NotMapped] public bool IsReserved =>null!=PaymentId && PaymentId.HasValue && PaymentId != Guid.Empty;

        public void SetPaymentInfo(Guid paymentId)
        {
            PaymentId = paymentId;
            Status = PrescriptionStatus.Active;
        }

        public void Close()
        {
            Status = PrescriptionStatus.Closed;
        }

        public bool HasNoAssignedIds()
        {
            return OrderItems.Any(x => x.PrescriptionOrderId != Id);
        }
        public void AssignIds()
        {
            OrderItems.ForEach(x => x.PrescriptionOrderId = Id);
        }
    }
}
