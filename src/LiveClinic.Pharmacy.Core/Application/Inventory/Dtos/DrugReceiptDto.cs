using System;
using System.Collections.Generic;
using LiveClinic.Pharmacy.Core.Domain.Inventory;

namespace LiveClinic.Pharmacy.Core.Application.Inventory.Dtos
{
    public class DrugReceiptDto
    {
        public Guid DrugId { get; set; }
        public string BatchNo { get; set; }
        public double Quantity { get; set; }
        public string OrderRef { get; set; }

    }
}
