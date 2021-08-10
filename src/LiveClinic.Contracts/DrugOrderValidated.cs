using System;
using System.Collections.Generic;

namespace LiveClinic.Contracts
{
    public interface DrugOrderValidated
    {
        Guid OrderId { get; set; }
        string OrderNo { get;set;   }
        string Patient { get;   set;}
        string Provider { get; set;}
        List<OrderItem> Medications { get; set; }
    }
}
