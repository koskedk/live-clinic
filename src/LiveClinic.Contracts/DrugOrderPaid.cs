using System;

namespace LiveClinic.Contracts
{
    public interface DrugOrderPaid
    {
        Guid OrderId { get; set; }
        Guid InvoiceId { get;set;   }


    }
}
