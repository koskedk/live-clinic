using System.Collections.Generic;
using LiveClinic.Consultation.Core.Application.Prescriptions.Dtos;
using LiveClinic.Consultation.Core.Domain.Prescriptions;

namespace LiveClinic.Consultation.Infrastructure.Tests.TestArtifacts
{
    public class TestData
    {
        public static List<Prescription> CreateTestPrescriptions()
        {
            var dto = new PrescriptionDto() { Patient = "Test Patient", Provider = "Dr Wu Long" };
            dto.Medications.Add(new MedicationDto() { DrugCode = "PN", Quantity = 10, Days = 5 });
            var dto2 = new PrescriptionDto() { Patient = "Test2 Patient2", Provider = "Dr Wu Long" };
            dto2.Medications.Add(new MedicationDto() { DrugCode = "BF", Quantity = 10, Days = 5 });

            var testPrescriptions = new List<Prescription>()
            {
                Prescription.Generate(dto), Prescription.Generate(dto2)
            };
            return testPrescriptions;
        }
    }
}
