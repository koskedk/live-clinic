using System;

namespace LiveClinic.Contracts
{
    public interface OrderPaid
    {
        Guid OrderId { get; set; }
        Guid PaymentId { get;   }
    }
}
