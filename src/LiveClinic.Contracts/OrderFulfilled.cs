using System;

namespace LiveClinic.Contracts
{
    public interface OrderFulfilled
    {
        Guid OrderId { get; set; }
        DateTime FulfillmentDate { get; set; }
    }
}