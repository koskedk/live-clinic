using System.Collections.Generic;
using LiveClinic.Pharmacy.Core.Domain.Inventory;

namespace LiveClinic.Pharmacy.Infrastructure.Seed
{
    public static class DrugSeed
    {
        public static List<Drug> GetDrugs()
        {
            return new List<Drug>()
            {
                new Drug("PN","Panadol 500mg","Pfizer"),
                new Drug("BF","Brufen 500mg","Pfizer")
            };
        }
    }
}
