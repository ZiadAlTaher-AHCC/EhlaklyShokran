using EhlaklyShokran.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Domain.BarberServices.Cosmetics
{
    public class CosmeticErrors
    {
        public static readonly Error NameRequired =
            Error.Validation("Cosmetic.Name.Required", "Cosmetic name is required.");

        public static readonly Error CostInvalid =
            Error.Validation("Cosmetic.Cost.Invalid", "Cosmetic cost must be between 1 and 10,000.");

        public static readonly Error QuantityInvalid =
            Error.Validation("Cosmetic.Quantity.Invalid", "Quantity must be between 1 and 10.");
    }
}
