using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using LiveClinic.SharedKernel.Domain;

namespace LiveClinic.Pharmacy.Core.Domain.Inventory
{
    public class Drug : AggregateRoot<Guid>
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public string Manufacturer  { get; private set; }
        public ICollection<StockTransaction> Transactions { get; private set; } = new List<StockTransaction>();

        [NotMapped] public double QuantityIn => GetQuantityIn();
        [NotMapped] public double QuantityOut => GetQuantityOut();
        [NotMapped] public double QuantityStock => QuantityIn - QuantityOut;
        [NotMapped] public bool InStock => QuantityIn > QuantityOut;

        private Drug()
        {
        }
        public Drug(string code, string name,string manufacturer="")
        {
            Code = code;
            Name = name;
            Manufacturer = manufacturer;
        }
        public StockTransaction ReceiveStock(string batchNo,double quantity,string order="")
        {
            var tx = new StockTransaction(batchNo, Movement.Received, quantity, Id,order);
            Transactions.Add(tx);
            return tx;
        }

        public StockTransaction Dispense(string batchNo,double quantity,double days,string order="")
        {
            double totalQuantity = quantity * days;
            var tx = new StockTransaction(batchNo, Movement.Dispensed, totalQuantity, Id,order);
            Transactions.Add(tx);
            return tx;
        }
        private double GetQuantityIn()
        {
            return Transactions.Where(x => x.Movement == Movement.Received).Sum(x => x.Quantity);
        }
        private double GetQuantityOut()
        {
            return Transactions.Where(x => x.Movement == Movement.Dispensed).Sum(x => x.Quantity);
        }
        public bool IsStocked(double quantityPrescribed)
        {
            return QuantityStock >= quantityPrescribed;
        }
        public override string ToString()
        {
            return $"{Code}-{Name}";
        }
    }
}
