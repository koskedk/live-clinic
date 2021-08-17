using System;
using System.Collections.Generic;

namespace LiveClinic.Pharmacy.Core.Application.Orders.Dtos
{
    public class ActiveOrderDtos
    {
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderNo { get;set;   }
        public string Patient { get; set; }
        public string Provider { get; set; }
        public List<ActiveOrderItemDtos> OrderItems { get; set; } = new List<ActiveOrderItemDtos>();
    }

    public class ActiveOrderItemDtos
    {
        public string DrugCode { get; set; }
        public double Days { get; set; }
        public double Quantity { get; set; }
        public double QuantityDispensed { get; set; }
        public Guid? DrugId { get; set; }
        public Guid PrescriptionOrderId { get; set; }
        public double QuantityPrescribed { get; set; }
    }
}
