using System.Collections.Generic;
using System.Linq;
using FizzWare.NBuilder;
using LiveClinic.Pharmacy.Core.Domain.Inventory;
using LiveClinic.Pharmacy.Core.Domain.Orders;

namespace LiveClinic.Pharmacy.Core.Tests.TestArtifacts
{
    public class TestData
    {

        public static List<PrescriptionOrder> CreateTestPrescriptionOrder(List<Drug> drugs,int orderCount=2,int itemCount =1)
        {
            var orders=Builder<PrescriptionOrder>.CreateListOfSize(orderCount).Build().ToList();
            int i = 0;
            foreach (var order in orders)
            {
                order.OrderItems=Builder<PrescriptionOrderItem>.CreateListOfSize(itemCount)
                    .All()
                    .With(x=>x.DrugCode=drugs[i].Code)
                    .With(x=>x.PrescriptionOrderId=order.Id)
                    .Build().ToList();
                i++;
            }
            return orders;
        }

        public static List<Drug> CreateTestDrugs(string code="T")
        {
            var testDrugs = new List<Drug>()
            {
                new Drug($"{code}1",$"Test1{code}"),
                new Drug($"{code}T2",$"Test2{code}")
            };

            foreach (var drug in testDrugs)
            {
                drug.ReceiveStock($"{drug.Code}B1", 10);
                drug.ReceiveStock($"{drug.Code}B2", 10);
                drug.Dispense($"{drug.Code}B2", 5);
            }

            return testDrugs;
        }

        public static Drug CreateTestDrugWithStock(string code="T",int initial=20)
        {
            var testDrug = new Drug($"{code}1", $"Test1{code}");
            testDrug.ReceiveStock($"LEO", initial);
            return testDrug;
        }
    }
}
