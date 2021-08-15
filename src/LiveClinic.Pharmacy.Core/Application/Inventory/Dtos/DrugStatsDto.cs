namespace LiveClinic.Pharmacy.Core.Application.Inventory.Dtos
{
    public class DrugStatsDto
    {
        public string Drug  { get;set;  }
        public double QuantityInStock  { get;set;  }
        public double TotalIn { get;set;  }
        public double TotalOut { get;set;  }

        public DrugStatsDto()
        {
        }
        public DrugStatsDto(InventoryDto inventoryDto)
        {
            Drug = inventoryDto.Name;
            QuantityInStock = inventoryDto.QuantityStock;
            TotalIn = inventoryDto.QuantityIn;
            TotalOut = inventoryDto.QuantityOut;
        }

        public override string ToString()
        {
            return $"{Drug} [{QuantityInStock}]";
        }
    }
}