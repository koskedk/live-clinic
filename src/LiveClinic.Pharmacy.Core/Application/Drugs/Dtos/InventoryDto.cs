using System;
using System.Collections.Generic;

namespace LiveClinic.Pharmacy.Core.Application.Dtos
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
        public List<StockTransactionDto> Transactions { get;set;} = new List<StockTransactionDto>();

        public override string ToString()
        {
            return $"{Name} Stock:{QuantityStock}";
        }
    }
}
