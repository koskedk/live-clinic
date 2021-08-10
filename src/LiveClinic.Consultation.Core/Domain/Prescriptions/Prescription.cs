using System;
using System.Collections.Generic;
using System.Linq;
using LiveClinic.Consultation.Core.Application.Prescriptions.Dtos;
using LiveClinic.SharedKernel;
using LiveClinic.SharedKernel.Domain;

namespace LiveClinic.Consultation.Core.Domain.Prescriptions
{
    public class Prescription : AggregateRoot<Guid>
    {
        public string Patient { get;private set; }
        public string Provider { get;private set; }
        public string OrderNo { get; private set;}
        public DateTime OrderDate { get; private set; }
        public OrderStatus Status { get; private set; }
        public List<Medication> Medications { get; private set;} = new List<Medication>();

        private Prescription()
        {
        }

        private Prescription(string patient, string provider)
        {
            Patient = patient;
            Provider = provider;
            OrderNo = Utils.GenerateNo("P");
            OrderDate = DateTime.Now;
            Status = OrderStatus.Created;
        }

        public static Prescription Generate(PrescriptionDto orderDto)
        {
            var order =new Prescription(orderDto.Patient,orderDto.Provider);
            order.AddDrugsToOrder(orderDto.Medications);

            if (!order.Medications.Any())
                throw new Exception($"Invalid order ! No drugs prescribed");

            return order;
        }

        public void ChangeStatus(OrderStatus status)
        {
            Status = status;
        }

        private void AddDrug(string drug, double days, double quantity)
        {
            if (Medications.Any(x => x.DrugCode == drug))
                throw new Exception($"Drug {drug} already Exists");
            Medications.Add(new Medication(drug, days, quantity, Id));
        }

        private void AddDrugsToOrder(List<MedicationDto> medicationDtos)
        {
            foreach (var medicationDto in medicationDtos)
                AddDrug(medicationDto.DrugCode,medicationDto.Days,medicationDto.Quantity);
        }

        public override string ToString()
        {
            return $"{OrderNo}|{Patient}|{Provider}";
        }
    }
}
