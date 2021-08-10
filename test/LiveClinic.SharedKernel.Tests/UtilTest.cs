using NUnit.Framework;
using Serilog;

namespace LiveClinic.SharedKernel.Tests
{
    public class UtilTests
    {
        [Test]
        public void should_GenerateNo()
        {
            var num1 = Utils.GenerateNo("P");
            var num2 = Utils.GenerateNo("P");
            Assert.AreNotEqual(num1,num2);
            Log.Debug($"{num1}");
            Log.Debug($"{num2}");
        }
    }
}
