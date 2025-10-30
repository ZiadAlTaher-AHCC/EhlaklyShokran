using EhlaklyShokran.Domain.BarberTasks;
using EhlaklyShokran.Domain.BarberTasks.Cosmetics;
using EhlaklyShokran.Domain.Customers;
using EhlaklyShokran.Domain.Employees;
using EhlaklyShokran.Domain.Identity;
using EhlaklyShokran.Domain.WorkOrders;
using EhlaklyShokran.Domain.WorkOrders.Billing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<Customer> Customers { get; }
        public DbSet<Cosmetic> Cosmetics { get; }
        public DbSet<BarberTask> BarberTasks { get; }
        public DbSet<WorkOrder> WorkOrders { get; }
        public DbSet<Employee> Employees { get; }
        public DbSet<Invoice> Invoices { get; }
        public DbSet<RefreshToken> RefreshTokens { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
