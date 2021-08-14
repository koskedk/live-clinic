using System;
using System.Collections.Generic;
using LiveClinic.Pharmacy.Core.Domain.Inventory;

namespace LiveClinic.Pharmacy.Core.Application.Inventory.Dtos
{
    public class AdjustStockDto
    {
        public Guid DrugId { get; set; }
        public string BatchNo { get; set; }
        public double Quantity { get; set; }

        public List<NewStockDto> Generate()
        {
            var list = new List<NewStockDto>();
            list.Add(new NewStockDto()
            {
                DrugId = DrugId, BatchNo = BatchNo, Quantity = Quantity, OrderRef = "", Movement = Movement.Received
            });
            return list;
        }
    }

}
