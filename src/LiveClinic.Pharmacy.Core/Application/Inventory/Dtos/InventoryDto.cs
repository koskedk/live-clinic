using System;
using System.Collections.Generic;
using LiveClinic.Pharmacy.Core.Domain.Inventory;

namespace LiveClinic.Pharmacy.Core.Application.Inventory.Dtos
{
    public class InventoryDto
    {
        public Guid Id { get;set;  }
        public string Code { get;set;  }
        public string Name { get; set; }
        public double QuantityIn { get; set; }
        public double QuantityOut { get; set; }
        public double QuantityStock { get; set; }
        public bool InStock { get; set; }
        public string IsStocked => InStock ? "Yes" : "No";
        public List<StockTransactionDto> Transactions { get;set;} = new List<StockTransactionDto>();

        public override string ToString()
        {
            return $"{Name} Stock:{QuantityStock}";
        }
    }

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
