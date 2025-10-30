using EhlaklyShokran.Domain.Common;
using EhlaklyShokran.Domain.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Domain.BarberTasks.Cosmetics
{
    public sealed class Cosmetic : AuditableEntity
    {
        public string? Name { get; private set; }
        public decimal Cost { get; private set; }
        public int Quantity { get; private set; }

        private Cosmetic()
        { }

        private Cosmetic(Guid id, string name, decimal cost, int quantity)
            : base(id)
        {
            Name = name;
            Cost = cost;
            Quantity = quantity;
        }

        public Result<Updated> Update(string? name, decimal cost, int quantity)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return CosmeticErrors.NameRequired;
            }

            if (cost <= 0 || cost > 10000)
            {
                return CosmeticErrors.CostInvalid;
            }

            if (quantity <= 0 || quantity > 10)
            {
                return CosmeticErrors.QuantityInvalid;
            }

            Name = name.Trim();
            Cost = cost;
            Quantity = quantity;

            return Result.Updated;
        }

        public static Result<Cosmetic> Create(Guid id, string name, decimal cost, int quantity)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return CosmeticErrors.NameRequired;
            }

            if (cost <= 0 || cost > 10000)
            {
                return CosmeticErrors.CostInvalid;
            }

            if (quantity <= 0 || quantity > 10)
            {
                return CosmeticErrors.QuantityInvalid;
            }

            return new Cosmetic(id, name.Trim(), cost, quantity);
        }
    }
}
