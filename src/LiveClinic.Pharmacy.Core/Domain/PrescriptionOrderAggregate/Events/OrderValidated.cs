using System;
using MediatR;

namespace LiveClinic.Pharmacy.Core.Domain.DrugAggregate.Events
{
    public class OrderValidated : INotification
    {
        public Guid PrescriptionOrderId  { get; }
        public bool IsAvailable { get; }
        public DateTime TimeStamp { get; } = new DateTime();

        public OrderValidated(Guid prescriptionOrderId, bool isAvailable)
        {
            PrescriptionOrderId = prescriptionOrderId;
            IsAvailable = isAvailable;
        }
    }
}
