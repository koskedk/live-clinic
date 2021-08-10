using System.Collections.Generic;

namespace LiveClinic.Consultation.Core.Application.Prescriptions.Dtos
{
    public class PrescriptionDto
    {
        public string Patient { get; set; }
        public string Provider { get; set; }
        public List<MedicationDto> Medications { get; set; }=new List<MedicationDto>();
    }
}
