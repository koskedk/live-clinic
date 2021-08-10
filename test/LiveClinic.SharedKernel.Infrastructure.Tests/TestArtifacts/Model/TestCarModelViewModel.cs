namespace LiveClinic.SharedKernel.Infrastructure.Tests.TestArtifacts.Model
{
    public class TestCarModelViewModel
    {
        public string Name { get; set; }
        public int Year { get; set; }

        public override string ToString()
        {
            return $"{Name} {Year}";
        }
    }
}