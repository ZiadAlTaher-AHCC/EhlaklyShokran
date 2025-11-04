using EhlaklyShokran.Application.Common.Interfaces;
using EhlaklyShokran.Domain.BarberTasks;
using EhlaklyShokran.Domain.BarberTasks.Cosmetics;
using EhlaklyShokran.Domain.Common;
using EhlaklyShokran.Domain.Customers;
using EhlaklyShokran.Domain.Employees;
using EhlaklyShokran.Domain.Identity;
using EhlaklyShokran.Domain.WorkOrders;
using EhlaklyShokran.Domain.WorkOrders.Billing;
using EhlaklyShokran.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EhlaklyShokran.Infrastructure.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IMediator mediator) : IdentityDbContext<ApplicationUser>(options), IApplicationDbContext
    {

        public DbSet<Cosmetic> Cosmetics => Set<Cosmetic>();
        public DbSet<BarberTask> BarberTasks => Set<BarberTask>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<WorkOrder> WorkOrders => Set<WorkOrder>();
        public DbSet<Invoice> Invoices => Set<Invoice>();
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await DispatchDomainEventsAsync(cancellationToken);
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }

        private async Task DispatchDomainEventsAsync(CancellationToken cancellationToken)
        {
            var domainEntities = ChangeTracker.Entries()
                .Where(e => e.Entity is Entity baseEntity && baseEntity.DomainEvents.Count != 0)
                .Select(e => (Entity)e.Entity)
                .ToList();

            var domainEvents = domainEntities
                .SelectMany(e => e.DomainEvents)
                .ToList();

            foreach (var domainEvent in domainEvents)
            {
                await mediator.Publish(domainEvent, cancellationToken);
            }

            foreach (var entity in domainEntities)
            {
                entity.ClearDomainEvents();
            }
        }
    }
}
