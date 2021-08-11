using System;
using MediatR;

namespace LiveClinic.Consultation.Core.Domain.Prescriptions.Events
{
    public class PrescriptionStatusChanged : INotification
    {
        public Guid Id { get; }
        public OrderStatus Status { get; }
        public DateTime TimeStamp { get; }

        public PrescriptionStatusChanged(Guid id, OrderStatus status)
        {
            Id = id;
            Status = status;
            TimeStamp=DateTime.Now;
        }
    }
}