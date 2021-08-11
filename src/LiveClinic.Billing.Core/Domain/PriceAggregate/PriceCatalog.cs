using System;
using System.Collections.Generic;
using LiveClinic.Billing.Core.Domain.InvoiceAggregate;
using LiveClinic.SharedKernel.Common;
using LiveClinic.SharedKernel.Domain;

namespace LiveClinic.Billing.Core.Domain.PriceAggregate
{
    public class PriceCatalog : AggregateRoot<Guid>
    {
        public Guid DrugId { get; set; }
        public string DrugCode { get; set; }
        public string Name { get; set; }
        public Money UnitPrice { get; set; }
        public List<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();

        public override string ToString()
        {
            return $"{Name} @ {UnitPrice}";
        }
    }
}
