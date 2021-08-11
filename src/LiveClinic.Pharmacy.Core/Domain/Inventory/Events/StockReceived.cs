using System;
using MediatR;

namespace LiveClinic.Pharmacy.Core.Domain.Inventory.Events
{
    public class StockReceived : INotification
    {
        public Guid StockId { get; }
        public DateTime TimeStamp { get; } = new DateTime();

        public StockReceived(Guid stockId)
        {
            StockId = stockId;
        }
    }
}
