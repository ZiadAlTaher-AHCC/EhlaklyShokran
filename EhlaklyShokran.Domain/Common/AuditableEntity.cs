using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Domain.Common
{
    public class AuditableEntity :Entity
    {
        protected AuditableEntity()
        { }

        protected AuditableEntity(Guid id)
            : base(id)
        {
        }

        public DateTimeOffset CreatedAtUtc { get; set; } // in UTC time to enable consistent time tracking across different time zones

        public string? CreatedBy { get; set; }

        public DateTimeOffset LastModifiedUtc { get; set; }

        public string? LastModifiedBy { get; set; }
    }
}
