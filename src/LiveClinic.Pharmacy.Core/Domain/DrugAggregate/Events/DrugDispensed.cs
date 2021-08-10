using System;
using MediatR;

namespace LiveClinic.Pharmacy.Core.Domain.DrugAggregate.Events
{
    public class DrugDispensed : INotification
    {
        public Guid StockId { get; }
        public DateTime TimeStamp { get; } = new DateTime();

        public DrugDispensed(Guid stockId)
        {
            StockId = stockId;
        }
    }
}
