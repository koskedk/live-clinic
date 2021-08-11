using System;
using System.Collections.Generic;
using System.Linq;

namespace LiveClinic.Billing.Core.Application.Invoicing.Dtos
{
    public class OrderInvoiceDto
    {
        public string Patient { get; set; }
        public Guid OrderId { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public string Provider { get; set;}
        public List<OrderInvoiceItemDto> OrderItems { get; set; } = new List<OrderInvoiceItemDto>();
        public List<string> DrugCodes => OrderItems.Select(x => x.DrugCode).ToList();
    }
}
