using System;
using MediatR;

namespace LiveClinic.Billing.Core.Domain.InvoiceAggregate.Events
{
    public class PaymentReceived : INotification
    {
        public Guid OrderId { get; }
        public Guid InvoiceId { get; }
        public Guid ReceiptId { get; }
        public DateTime TimeStamp { get; }

        public PaymentReceived(Guid orderId, Guid invoiceId, Guid receiptId)
        {
            OrderId = orderId;
            InvoiceId = invoiceId;
            ReceiptId = receiptId;
            TimeStamp = DateTime.Now;
        }
    }
}
