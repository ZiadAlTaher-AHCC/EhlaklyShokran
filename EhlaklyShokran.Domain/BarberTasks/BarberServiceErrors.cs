using EhlaklyShokran.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Domain.BarberTasks
{

    public static class BarberTaskErrors
    {
        public static Error NameRequired =>
            Error.Validation("BarberTask.Name.Required", "Name is required.");

        public static Error LaborCostInvalid =>
            Error.Validation("BarberTask.LaborCost.Invalid", "Labor cost must be between 1 and 10,000.");

        public static Error DurationInvalid =>
            Error.Validation("BarberTask.Duration.Invalid", "Invalid duration selected.");

        public static Error CosmeticsRequired =>
            Error.Validation("BarberTask.Cosmetics.Required", "At least one cosmetic is required.");

        public static Error CosmeticNameRequired =>
            Error.Validation("BarberTask.Cosmetics.Name.Required", "All cosmetics must have a name.");

        public static Error AtLeastOneBarberTaskIsRequired =>
              Error.Validation(
                  code: "BarberTask.Required",
                  description: "At least one Barber task must be specified.");

        public static Error InUse =>
        Error.Conflict("BarberTask.InUse", "Cannot delete a Barber task that is used in work orders.");

        public static Error DuplicateName =>

        Error.Conflict("BarberTaskCosmetic.Duplicate", "A cosmetic with the same name already exists in this Barber task.");
    }
}
