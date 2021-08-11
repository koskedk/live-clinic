using System;
using MediatR;

namespace LiveClinic.Pharmacy.Core.Domain.Inventory.Events
{
    public class DrugsDispensed : INotification
    {
        public Guid OrderId { get; }
        public DateTime TimeStamp { get;  }

        public DrugsDispensed(Guid orderId)
        {
            OrderId = orderId;
            TimeStamp=DateTime.Now;
        }
    }
}
