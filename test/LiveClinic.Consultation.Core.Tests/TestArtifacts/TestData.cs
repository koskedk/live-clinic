using System.Collections.Generic;
using LiveClinic.Consultation.Core.Application.Prescriptions.Dtos;
using LiveClinic.Consultation.Core.Domain.Prescriptions;

namespace LiveClinic.Consultation.Core.Tests.TestArtifacts
{
    public class TestData
    {
        public static List<Prescription> CreateTestPrescriptions()
        {
            var dto = new PrescriptionDto() {Patient = "Test Patient", Provider = "Dr Wu Long"};
            dto.Medications.Add(new MedicationDto(){DrugCode = "P",Quantity = 10,Days = 5});
            var dto2 = new PrescriptionDto() {Patient = "Test2 Patient2", Provider = "Dr Wu Long"};
            dto2.Medications.Add(new MedicationDto(){DrugCode = "B",Quantity = 10,Days = 5});

            var testDrugOrders = new List<Prescription>()
            {
             Prescription.Generate(dto),Prescription.Generate(dto2)
            };
            return testDrugOrders;
        }

        public static PrescriptionDto CreateTestPrescriptionDto(string code="PN")
        {
            var dto = new PrescriptionDto() {Patient = "Test Patient", Provider = "Dr Wu Long"};
            dto.Medications.Add(new MedicationDto(){DrugCode = code,Quantity = 10,Days = 5});
            return dto;
        }
    }
}
