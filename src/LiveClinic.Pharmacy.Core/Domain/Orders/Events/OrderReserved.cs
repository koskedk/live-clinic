using System;
using MediatR;

namespace LiveClinic.Pharmacy.Core.Domain.Orders.Events
{
    public class OrderReserved : INotification
    {
        public Guid OrderId { get; }
        public DateTime TimeStamp { get; } = new DateTime();

        public OrderReserved(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
