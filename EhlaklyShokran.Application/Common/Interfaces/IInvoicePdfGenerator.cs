using EhlaklyShokran.Domain.WorkOrders.Billing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Application.Common.Interfaces
{
    public interface IInvoicePdfGenerator
    {
        byte[] Generate(Invoice invoice);
    }
}
