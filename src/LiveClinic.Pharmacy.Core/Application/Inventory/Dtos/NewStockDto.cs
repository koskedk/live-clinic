using System;
using LiveClinic.Pharmacy.Core.Domain.Inventory;

namespace LiveClinic.Pharmacy.Core.Application.Inventory.Dtos
{
    public class NewStockDto
    {
        public Guid DrugId { get; set; }
        public string BatchNo { get;set; }
        public double Quantity { get; set;}
        public string OrderRef { get; set;}
        public Movement Movement { get; set; } = Movement.Received;
    }
}
