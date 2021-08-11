using System.Collections.Generic;
using LiveClinic.Billing.Core.Domain.PriceAggregate;
using LiveClinic.SharedKernel.Common;

namespace LiveClinic.Billing.Infrastructure.Seed
{
    public static class PriceCatalogSeed
    {
        public static List<PriceCatalog> GetCatalogs()
        {
            return new()
            {
                new(){DrugCode = "PN",Name ="Panadol 500mg",UnitPrice = new Money(10,"KES")},
                new(){DrugCode = "BF",Name ="Brufen 500mg",UnitPrice = new Money(10,"KES") }
            };
        }
    }
}
