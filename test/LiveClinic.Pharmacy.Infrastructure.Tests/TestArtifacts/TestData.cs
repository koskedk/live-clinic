using System.Collections.Generic;
using System.Linq;
using FizzWare.NBuilder;
using LiveClinic.Pharmacy.Core.Domain.Inventory;
using LiveClinic.Pharmacy.Core.Domain.Orders;

namespace LiveClinic.Pharmacy.Infrastructure.Tests.TestArtifacts
{
    public class TestData
    {
        public static List<PrescriptionOrder> CreateTestPrescriptionOrder(List<Drug> drugs,int orderCount=2,int itemCount =2)
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
            }
            return orders;
        }
    }
}
