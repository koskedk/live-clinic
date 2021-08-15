using System.Collections.Generic;
using System.Linq;

namespace LiveClinic.Pharmacy.Core.Application.Inventory.Dtos
{
    public class InventoryStatsDto
    {
        public int TotalDrugs { get;set;  }
        public int TotalInStock { get;set;  }
        public int TotalOutOfStock { get;set;  }
        public List<DrugStatsDto> InStockDrugStats { get; set; } = new List<DrugStatsDto>();
        public List<DrugStatsDto> OutOfStockDrugStats { get; set; } = new List<DrugStatsDto>();

        public static InventoryStatsDto Generate(List<InventoryDto> inventoryDtos)
        {
            var stats = new InventoryStatsDto();
            stats.TotalDrugs = inventoryDtos.Count;
            stats.TotalInStock = inventoryDtos.Count(x => x.QuantityStock > 0);
            stats.TotalOutOfStock = inventoryDtos.Count(x => x.QuantityStock <= 0);
            stats.InStockDrugStats = inventoryDtos.Where(x=>x.InStock).Select(x => new DrugStatsDto(x)).ToList();
            stats.OutOfStockDrugStats = inventoryDtos.Where(x=>!x.InStock).Select(x => new DrugStatsDto(x)).ToList();
            return stats;
        }

        public override string ToString()
        {
            return $"{TotalDrugs} Items | {TotalInStock} Available |{TotalOutOfStock} Out Of Stock";
        }
    }
}
