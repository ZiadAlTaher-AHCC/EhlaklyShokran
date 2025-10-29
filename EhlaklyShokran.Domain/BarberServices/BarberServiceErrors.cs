using EhlaklyShokran.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Domain.BarberServices
{

    public static class BarberServiceErrors
    {
        public static Error NameRequired =>
            Error.Validation("BarberService.Name.Required", "Name is required.");

        public static Error LaborCostInvalid =>
            Error.Validation("BarberService.LaborCost.Invalid", "Labor cost must be between 1 and 10,000.");

        public static Error DurationInvalid =>
            Error.Validation("BarberService.Duration.Invalid", "Invalid duration selected.");

        public static Error PartsRequired =>
            Error.Validation("BarberService.Parts.Required", "At least one part is required.");

        public static Error PartNameRequired =>
            Error.Validation("BarberService.Parts.Name.Required", "All parts must have a name.");

        public static Error AtLeastOneBarberServiceIsRequired =>
              Error.Validation(
                  code: "BarberService.Required",
                  description: "At least one repair task must be specified.");

        public static Error InUse =>
        Error.Conflict("BarberService.InUse", "Cannot delete a repair task that is used in work orders.");

        public static Error DuplicateName =>

        Error.Conflict("BarberServicePart.Duplicate", "A part with the same name already exists in this repair task.");
    }
}
