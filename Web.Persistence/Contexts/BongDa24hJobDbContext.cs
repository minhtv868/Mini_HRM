using Web.Application.Interfaces.Repositories.BongDa24hJobs;
using Web.Domain.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Web.Persistence.Contexts
{
    public class BongDa24hJobDbContext(DbContextOptions<BongDa24hJobDbContext> options,
          IDomainEventDispatcher dispatcher) : DbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly(),
                            t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfigurationBongDa24hJobDbContext<>)));
        }
    }
}
