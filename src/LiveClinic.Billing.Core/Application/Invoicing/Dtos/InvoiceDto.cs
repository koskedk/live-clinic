using System;
using System.Collections.Generic;

namespace LiveClinic.Billing.Core.Application.Invoicing.Dtos
{
    public class InvoiceDto
    {
        public string Patient { get; set; }
        public Guid OrderId { get; set; }
        public string OrderNo { get; set; }
        DateTime OrderDate { get; set; }
        string Provider { get; set;}
        public List<InvoiceItemDto> OrderItems { get; set; }=new List<InvoiceItemDto>();
    }
}
