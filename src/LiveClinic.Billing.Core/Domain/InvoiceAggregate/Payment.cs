using System;
using LiveClinic.Billing.Core.Domain.Common;
using LiveClinic.SharedKernel;
using LiveClinic.SharedKernel.Domain;

namespace LiveClinic.Billing.Core.Domain.InvoiceAggregate
{
    public class Payment:Entity<Guid>
    {
        public DateTime ReceiptDate {get;private set;}
        public string ReceiptNo { get; private set; }
        public Money AmountPaid {get;private set;}
        public Guid InvoiceId {get;private set;}

        private Payment()
        {
        }

        public Payment(Money amountPaid, Guid invoiceId)
        {
            ReceiptNo = Utils.GenerateNo("R");
            AmountPaid = amountPaid;
            InvoiceId = invoiceId;
            ReceiptDate=DateTime.Now;
        }
    }
}
