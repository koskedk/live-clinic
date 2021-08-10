using System;

namespace LiveClinic.Billing.Core.Application.Invoicing.Dtos
{
    public class PaymentDto
    {
        public double Amount {get; set;}
        public Guid InvoiceId {get; set;}
    }
}
