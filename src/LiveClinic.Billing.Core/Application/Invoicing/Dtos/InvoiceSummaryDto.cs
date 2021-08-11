using System;
using System.Collections.Generic;
using LiveClinic.Billing.Core.Domain.InvoiceAggregate;
using LiveClinic.SharedKernel.Common;

namespace LiveClinic.Billing.Core.Application.Invoicing.Dtos
{
    public class InvoiceSummaryDto
    {
        public Guid Id { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string Patient { get; set; }
        public InvoiceStatus Status { get; set; }
        public Money TotalAmount { get; set; }
        public Money TotalPaid { get; set; }
        public Money Balance { get; set; }
        public string StatusName => $"{Status}";
        public List<InvoiceLineDto> Items { get; private set; } = new List<InvoiceLineDto>();
        public List<InvoicePaymentDto> Payments { get; private set; } = new List<InvoicePaymentDto>();

        public override string ToString()
        {
            return $"{InvoiceNo} {TotalAmount} {Status}";
        }
    }

    public class InvoiceLineDto
    {
        public string Item { get;   set;}
        public string DrugCode  { get; set; }
        public double Quantity  { get;   set;}
        public Money QuotePrice  { get; set;}

        public override string ToString()
        {
            return $"{Item} {Quantity} {QuotePrice}";
        }
    }
    public class InvoicePaymentDto
    {
        public DateTime ReceiptDate {get;private set;}
        public string ReceiptNo { get; private set; }
        public Money AmountPaid {get;private set;}

        public override string ToString()
        {
            return $"{ReceiptNo} {AmountPaid}";
        }
    }
}
