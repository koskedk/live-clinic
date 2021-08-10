using System;
using MediatR;

namespace LiveClinic.Consultation.Core.Domain.Prescriptions.Events
{
    public class PrescriptionGenerated : INotification
    {
        public Guid Id { get; }
        public DateTime TimeStamp { get; } = new DateTime();
        public PrescriptionGenerated(Guid id)
        {
            Id = id;
        }
    }
}
