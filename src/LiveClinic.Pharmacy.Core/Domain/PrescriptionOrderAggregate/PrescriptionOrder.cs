using System;
using System.Collections.Generic;
using System.Linq;
using LiveClinic.SharedKernel.Domain;

namespace LiveClinic.Pharmacy.Core.Domain.PrescriptionOrderAggregate
{
    public class PrescriptionOrder:AggregateRoot<Guid>
    {
        public Guid OrderId { get; set; }
        public string OrderNo { get;set;   }
        public string Patient { get; set; }
        public string Provider { get; set; }
        public Guid? PaymentId { get; set; }
        public List<PrescriptionOrderItem> OrderItems { get; set; } = new List<PrescriptionOrderItem>();
        public void SetPaymentInfo(Guid paymentId)
        {
            PaymentId = paymentId;
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
