using System;

namespace LiveClinic.Pharmacy.Core.Application.Dtos
{
    public class NewStockDto
    {
        public Guid DrugId { get; set; }
        public string BatchNo { get;set; }
        public double Quantity { get; set;}
        public string OrderRef { get; set;}
    }
}
