using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using LiveClinic.Billing.Core.Application.Invoicing.Dtos;
using LiveClinic.Billing.Core.Domain.PriceAggregate;
using LiveClinic.SharedKernel;
using LiveClinic.SharedKernel.Common;
using LiveClinic.SharedKernel.Domain;

namespace LiveClinic.Billing.Core.Domain.InvoiceAggregate
{
    public class Invoice : AggregateRoot<Guid>
    {
        public string InvoiceNo { get; private set;}
        public DateTime InvoiceDate { get;private set; }
        public string Patient { get;private set; }
        public Guid OrderId { get; private set;}
        public string OrderNo { get; private set; }
        public InvoiceStatus Status { get; private set; }
        public List<InvoiceItem> Items { get; private set;} = new List<InvoiceItem>();
        public List<Payment> Payments { get;private set; }=new List<Payment>();
        [NotMapped] public Money TotalAmount => GetTotalValue();
        [NotMapped] public Money TotalPaid => GetPaidValue();
        [NotMapped] public Money Balance => GetBalanceValue();

        private Invoice()
        {
        }

        private Invoice(string patient, Guid orderId, string orderNo)
        {
            Patient = patient;
            OrderId = orderId;
            OrderNo = orderNo;
            InvoiceNo = Utils.GenerateNo("I");
            InvoiceDate = DateTime.Now;
            Status = InvoiceStatus.NotPaid;
        }

        public static Invoice Generate(OrderInvoiceDto invoiceDto, List<PriceCatalog> prices)
        {
            var inv = new Invoice(invoiceDto.Patient, invoiceDto.OrderId, invoiceDto.OrderNo);

            inv.AddItems(invoiceDto.OrderItems,prices);

            return inv;
        }

        public static Invoice Generate(InvoiceDto invoiceDto, List<PriceCatalog> prices)
        {
            var inv = new Invoice(invoiceDto.Patient, invoiceDto.OrderId, invoiceDto.OrderNo);

            inv.AddItems(invoiceDto.OrderItems,prices);

            return inv;
        }


        public void MakePayment(Payment payment)
        {
            if (payment.InvoiceId != Id)
                throw new Exception("payment mismatch");

            Payments.Add(payment);
            Status = Balance.Amount == 0 ? InvoiceStatus.Paid : InvoiceStatus.NotPaid;
        }

        public void Clear()
        {
            Items = new List<InvoiceItem>();
            Payments = new List<Payment>();
        }

        private void AddItems(List<InvoiceItemDto> itemDtos,List <PriceCatalog> prices)
        {
            foreach (var dto in itemDtos)
            {
                var price = prices.FirstOrDefault(x => x.DrugCode == dto.DrugCode);
                Items.Add(new InvoiceItem(price.Id, dto.Quantity,dto.Days,  dto.UnitPrice, Id));
            }
        }

        private void AddItems(List<OrderInvoiceItemDto> itemDtos, List<PriceCatalog> prices)
        {
            foreach (var dto in itemDtos)
            {
                var price = prices.FirstOrDefault(x => x.DrugCode == dto.DrugCode);
                Items.Add(new InvoiceItem(price.Id, dto.Quantity,dto.Days,
                    null != price ? price.UnitPrice : new Money(0, "KES"), Id));
            }
        }

        private Money GetTotalValue()
        {
            var totalAmount = Items.Sum(x => x.LineTotal.Amount);
            return new Money(totalAmount, GetItemPriceCurrency());
        }
        private Money GetPaidValue()
        {
            var totalAmount = Payments.Any() ? Payments.Sum(x => x.AmountPaid.Amount) : 0;
            return new Money(totalAmount, GetPaymentCurrency());
        }

        private Money GetBalanceValue()
        {
            var totalAmount = TotalAmount.Amount - TotalPaid.Amount;
            return new Money(totalAmount, GetPaymentCurrency());
        }
        private string GetItemPriceCurrency()
        {
            return Items.Any() ? Items.First().QuotePrice.Currency : AppConstants.Currency;
        }
        private string GetPaymentCurrency()
        {
           return Payments.Any() ? Payments.First().AmountPaid.Currency : AppConstants.Currency;
        }
        public override string ToString()
        {
            return $"{InvoiceNo}|{Patient}|{Balance}|{Status}";
        }
    }
}
