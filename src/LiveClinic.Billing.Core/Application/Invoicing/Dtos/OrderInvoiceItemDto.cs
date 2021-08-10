using System;

namespace LiveClinic.Billing.Core.Application.Invoicing.Dtos
{
    public class OrderInvoiceItemDto
    {

        public Guid DrugId { get; set; }
        public string DrugCode  { get; set; }
        public double Quantity  { get; set; }
    }
}