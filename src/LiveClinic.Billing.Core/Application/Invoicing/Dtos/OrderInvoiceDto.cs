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
        public List<OrderInvoiceItemDto> Items { get; set; } = new List<OrderInvoiceItemDto>();
        public List<string> DrugCodes => Items.Select(x => x.DrugCode).ToList();
    }
}
