using System;
using System.ComponentModel.DataAnnotations.Schema;
using LiveClinic.SharedKernel.Domain;

namespace LiveClinic.SharedKernel.Infrastructure.Tests.TestArtifacts.Model
{
    [Table(nameof(TestCarModel))]
    public class TestCarModel : Entity<Guid>
    {
        public string Name { get; set; }
        public int Year { get; set; }
        public TestTrim Trim { get; set; } = new TestTrim();
        public Guid TestCarId { get; set; }

        public TestCarModel()
        {
        }

        public TestCarModel(Guid id) : base(id)
        {
        }

        public TestCarModel(string name, int year, TestTrim trim, Guid testCarEntityId)
        {
            Name = name;
            Year = year;
            Trim = trim;
            TestCarId = testCarEntityId;
        }


        public override string ToString()
        {
            return $"{Name} {Year} ({Trim})";
        }

        public void ChangeTrim(string transmission, string fuelType)
        {
            Trim.Transmission = transmission;
            Trim.FuelType = fuelType;
        }
    }
}
