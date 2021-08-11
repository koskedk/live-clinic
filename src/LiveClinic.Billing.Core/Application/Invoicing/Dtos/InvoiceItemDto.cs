using System;
using LiveClinic.SharedKernel.Common;

namespace LiveClinic.Billing.Core.Application.Invoicing.Dtos
{
    public class InvoiceItemDto
    {
        public Guid DrugId { get; set; }
        public string DrugCode  { get; set; }

        public string Name { get;  set; }
        public double Quantity  { get; set; }

        public Guid PriceCatalogId { get; set; }
        public Money UnitPrice { get; set; }
    }
}
