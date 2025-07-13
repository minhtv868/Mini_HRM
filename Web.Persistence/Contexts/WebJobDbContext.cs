//using Microsoft.EntityFrameworkCore;
//using System.Reflection;
//using Web.Application.Interfaces.Repositories.Finances;
//using Web.Domain.Common.Interfaces;

//namespace Web.Persistence.Contexts
//{
//    public class WebJobDbContext(DbContextOptions<WebJobDbContext> options,
//          IDomainEventDispatcher dispatcher) : DbContext(options)
//    {
//        protected override void OnModelCreating(ModelBuilder modelBuilder)
//        {
//            base.OnModelCreating(modelBuilder);

//            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly(),
//                            t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityTypeConfigurationFinanceDbContext<>)));
//        }
//    }
//}
