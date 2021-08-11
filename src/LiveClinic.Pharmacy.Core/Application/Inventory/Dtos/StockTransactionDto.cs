using System;
using LiveClinic.Pharmacy.Core.Domain.Inventory;

namespace LiveClinic.Pharmacy.Core.Application.Inventory.Dtos
{
    public class StockTransactionDto
    {
        public Guid Id { get; set; }
        public string BatchNo { get;set; }
        public Movement Movement { get;set;  }
        public DateTime MovementDate { get;set;  }
        public double Quantity { get; set; }
        public Guid DrugId { get;  set;}
    }
}
