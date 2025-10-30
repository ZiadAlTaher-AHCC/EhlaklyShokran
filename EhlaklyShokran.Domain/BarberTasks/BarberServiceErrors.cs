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

        public static Error PartsRequired =>
            Error.Validation("BarberTask.Parts.Required", "At least one part is required.");

        public static Error PartNameRequired =>
            Error.Validation("BarberTask.Parts.Name.Required", "All parts must have a name.");

        public static Error AtLeastOneBarberTaskIsRequired =>
              Error.Validation(
                  code: "BarberTask.Required",
                  description: "At least one repair task must be specified.");

        public static Error InUse =>
        Error.Conflict("BarberTask.InUse", "Cannot delete a repair task that is used in work orders.");

        public static Error DuplicateName =>

        Error.Conflict("BarberTaskPart.Duplicate", "A part with the same name already exists in this repair task.");
    }
}
