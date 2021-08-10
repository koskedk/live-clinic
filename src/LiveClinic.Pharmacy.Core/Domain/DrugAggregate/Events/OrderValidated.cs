using System;
using LiveClinic.Contracts;
using MediatR;

namespace LiveClinic.Pharmacy.Core.Domain.DrugAggregate.Events
{
    public class OrderValidated : INotification
    {
        public DrugOrderValidated DrugOrder { get; }
        public bool IsAvailable { get; }
        public DateTime TimeStamp { get; } = new DateTime();

        public OrderValidated(DrugOrderValidated order, bool isAvailable)
        {
            DrugOrder = order;
            IsAvailable = isAvailable;
        }
    }
}
