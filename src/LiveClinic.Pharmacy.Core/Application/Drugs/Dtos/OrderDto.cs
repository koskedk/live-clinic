using System;
using System.Collections.Generic;

namespace LiveClinic.Pharmacy.Core.Application.Dtos
{
    public class OrderDto
    {
        public Guid OrderId { get; private set; }
        public string OrderNo { get; private set; }
        public string Patient { get; private set; }
        public string Provider { get; private set; }
        public List<OrderItemDto> Items = new List<OrderItemDto>();
    }

    public class OrderItemDto
    {
        public Guid DrugId { get; private set; }
        public double QuantityPrescribed { get; private set; }
    }
}
