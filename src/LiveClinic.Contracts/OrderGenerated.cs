using System;
using System.Collections.Generic;

namespace LiveClinic.Contracts
{
    public interface OrderGenerated
    {
        Guid OrderId { get; set; }
        string OrderNo { get;set;   }
        DateTime OrderDate { get; set; }
        string Patient { get;   set;}
        string Provider { get; set;}
        List<OrderItem> OrderItems { get; set; }
    }

    public interface OrderItem
    {
        string DrugCode { get; set; }
        double Days { get; set; }
        double Quantity { get; set; }
    }
}
