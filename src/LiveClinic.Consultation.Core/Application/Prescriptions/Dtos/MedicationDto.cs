namespace LiveClinic.Consultation.Core.Application.Prescriptions.Dtos
{
    public class MedicationDto
    {
        public string DrugCode { get; set; }
        public double Days { get; set; }
        public double Quantity { get; set; }
    }
}
