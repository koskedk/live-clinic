using System;
using LiveClinic.Billing.Core.Domain.Common;

namespace LiveClinic.Billing.Core.Application.Invoicing.Dtos
{
    public class InvoiceItemDto
    {
        public Guid PriceCatalogId { get; set; }
        public double Quantity  { get; set; }
        public Guid DrugId { get; set; }
        public string DrugCode  { get; set; }
        public Money UnitPrice { get; set; }
    }
}
