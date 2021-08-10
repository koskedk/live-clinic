using System;
using MediatR;

namespace LiveClinic.Billing.Core.Domain.InvoiceAggregate.Events
{
    public class InvoiceGenerated : INotification
    {
        public Guid InvoiceId { get; }
        public DateTime TimeStamp { get; } = new DateTime();

        public InvoiceGenerated(Guid invoiceId)
        {
            InvoiceId = invoiceId;
        }
    }
}
