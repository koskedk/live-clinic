using System;
using System.Collections.Generic;

namespace LiveClinic.Pharmacy.Core.Application.Dtos
{
    public class StockOutDto
    {
        public Guid OrderId { get; set; }
        public string OrderNo { get; set; }
        public string Patient { get; set; }
        public string Provider { get; set; }
        public List<DrugDto> Medications { get; set; } = new List<DrugDto>();
    }

    public class DrugDto
    {
        public Guid DrugId { get; }
        public string BatchNo { get; }
        public double Quantity { get; }
        public string OrderRef { get; }
    }
}
