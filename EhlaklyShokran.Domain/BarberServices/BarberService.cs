using EhlaklyShokran.Domain.BarberServices.Cosmetics;
using EhlaklyShokran.Domain.BarberServices.Enums;
using EhlaklyShokran.Domain.Common;
using EhlaklyShokran.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Domain.BarberServices
{

    public sealed class BarberService : AuditableEntity
    {
        public string Name { get; private set; }
        public decimal LaborCost { get; private set; }
        public ServiceDurationInMinutes EstimatedDurationInMins { get; private set; }

        private readonly List<Cosmetic> _Cosmetics = [];
        public IEnumerable<Cosmetic> Cosmetics => _Cosmetics.AsReadOnly();
        public decimal TotalCost => LaborCost + Cosmetics.Sum(p => p.Cost * p.Quantity);

#pragma warning disable CS8618

        private BarberService()
        { }

#pragma warning restore CS8618

        private BarberService(Guid id, string name, decimal laborCost, ServiceDurationInMinutes estimatedDurationInMins, List<Cosmetic> Cosmetics)
            : base(id)
        {
            Name = name;
            LaborCost = laborCost;
            EstimatedDurationInMins = estimatedDurationInMins;
            _Cosmetics = Cosmetics;
        }

        public static Result<BarberService> Create(Guid id, string name, decimal laborCost, ServiceDurationInMinutes estimatedDurationInMins, List<Cosmetic> Cosmetics)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BarberServiceErrors.NameRequired;
            }

            if (laborCost <= 0)
            {
                return BarberServiceErrors.LaborCostInvalid;
            }

            if (!Enum.IsDefined(estimatedDurationInMins))
            {
                return BarberServiceErrors.DurationInvalid;
            }

            return new BarberService(id, name.Trim(), laborCost, estimatedDurationInMins, Cosmetics);
        }

        public Result<Updated> UpsertCosmetics(List<Cosmetic> incomingCosmetics)
        {
            _Cosmetics.RemoveAll(existing => incomingCosmetics.All(p => p.Id != existing.Id));

            foreach (var incoming in incomingCosmetics)
            {
                var existing = _Cosmetics.FirstOrDefault(p => p.Id == incoming.Id);
                if (existing is null)
                {
                    _Cosmetics.Add(incoming);
                }
                else
                {
                    var updateCosmeticResult = existing.Update(incoming.Name, incoming.Cost, incoming.Quantity);
                    if (updateCosmeticResult.IsError)
                    {
                        return updateCosmeticResult.Errors;
                    }
                }
            }

            return Result.Updated;
        }

        public Result<Updated> Update(string name, decimal laborCost, ServiceDurationInMinutes estimatedDurationInMins)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BarberServiceErrors.NameRequired;
            }

            if (laborCost <= 0 || laborCost > 10000)
            {
                return BarberServiceErrors.LaborCostInvalid;
            }

            if (!Enum.IsDefined(estimatedDurationInMins))
            {
                return BarberServiceErrors.DurationInvalid;
            }

            Name = name.Trim();
            LaborCost = laborCost;
            EstimatedDurationInMins = estimatedDurationInMins;

            return Result.Updated;
        }
    }
}
