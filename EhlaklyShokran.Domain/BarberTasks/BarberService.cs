using EhlaklyShokran.Domain.BarberTasks.Cosmetics;
using EhlaklyShokran.Domain.BarberTasks.Enums;
using EhlaklyShokran.Domain.Common;
using EhlaklyShokran.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Domain.BarberTasks
{

    public sealed class BarberTask : AuditableEntity
    {
        public string Name { get; private set; }
        public decimal LaborCost { get; private set; }
        public ServiceDurationInMinutes EstimatedDurationInMins { get; private set; }

        private readonly List<Cosmetic> _Cosmetics = [];
        public IEnumerable<Cosmetic> Cosmetics => _Cosmetics.AsReadOnly();
        public decimal TotalCost => LaborCost + Cosmetics.Sum(p => p.Cost * p.Quantity);

#pragma warning disable CS8618

        private BarberTask()
        { }

#pragma warning restore CS8618

        private BarberTask(Guid id, string name, decimal laborCost, ServiceDurationInMinutes estimatedDurationInMins, List<Cosmetic> Cosmetics)
            : base(id)
        {
            Name = name;
            LaborCost = laborCost;
            EstimatedDurationInMins = estimatedDurationInMins;
            _Cosmetics = Cosmetics;
        }

        public static Result<BarberTask> Create(Guid id, string name, decimal laborCost, ServiceDurationInMinutes estimatedDurationInMins, List<Cosmetic> Cosmetics)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BarberTaskErrors.NameRequired;
            }

            if (laborCost <= 0)
            {
                return BarberTaskErrors.LaborCostInvalid;
            }

            if (!Enum.IsDefined(estimatedDurationInMins))
            {
                return BarberTaskErrors.DurationInvalid;
            }

            return new BarberTask(id, name.Trim(), laborCost, estimatedDurationInMins, Cosmetics);
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
                return BarberTaskErrors.NameRequired;
            }

            if (laborCost <= 0 || laborCost > 10000)
            {
                return BarberTaskErrors.LaborCostInvalid;
            }

            if (!Enum.IsDefined(estimatedDurationInMins))
            {
                return BarberTaskErrors.DurationInvalid;
            }

            Name = name.Trim();
            LaborCost = laborCost;
            EstimatedDurationInMins = estimatedDurationInMins;

            return Result.Updated;
        }
    }
}
