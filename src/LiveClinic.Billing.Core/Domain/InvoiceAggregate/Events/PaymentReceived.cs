using System;
using MediatR;

namespace LiveClinic.Billing.Core.Domain.InvoiceAggregate.Events
{
    public class PaymentReceived : INotification
    {
        public Guid InvoiceId { get; }
        public Guid OrderId { get; }
        public DateTime TimeStamp { get; } = new DateTime();

        public PaymentReceived(Guid invoiceId, Guid orderId)
        {
            InvoiceId = invoiceId;
            OrderId = orderId;


        }
    }
}
