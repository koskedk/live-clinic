using System;
using System.ComponentModel.DataAnnotations.Schema;
using LiveClinic.Billing.Core.Domain.PriceAggregate;
using LiveClinic.SharedKernel.Common;
using LiveClinic.SharedKernel.Domain;

namespace LiveClinic.Billing.Core.Domain.InvoiceAggregate
{
    public class InvoiceItem:Entity<Guid>
    {
        public Guid PriceCatalogId { get; private set; }
        public double Quantity  { get;  private set;}
        public Money QuotePrice  { get;private set;}
        public Guid InvoiceId { get; private set;}
        [NotMapped]
        public Money LineTotal => CalcTotal();
        public virtual PriceCatalog PriceCatalog { get; private set; }

        private InvoiceItem()
        {
        }

        public InvoiceItem(Guid priceCatalogId, double quantity, double days, Money quotePrice, Guid invoiceId)
        {
            PriceCatalogId = priceCatalogId;
            Quantity = quantity * days;
            QuotePrice = quotePrice;
            InvoiceId = invoiceId;
        }

        private Money CalcTotal()
        {
            var total = Quantity * QuotePrice.Amount;
            return new Money(total, QuotePrice.Currency);
        }
    }
}
