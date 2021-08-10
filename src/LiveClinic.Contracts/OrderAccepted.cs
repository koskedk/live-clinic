using System;
using System.Collections.Generic;

namespace LiveClinic.Contracts
{
    public interface OrderAccepted
    {
        Guid OrderId { get; set; }
        string OrderNo { get;set;   }
        DateTime OrderDate { get; set; }
        string Patient { get;   set;}
        string Provider { get; set;}
        List<OrderItem> OrderItems { get; set; }
    }
}
