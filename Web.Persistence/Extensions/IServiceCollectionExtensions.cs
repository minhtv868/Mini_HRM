using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Web.Application.Interfaces;
using Web.Application.Interfaces.Repositories;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Application.Interfaces.Repositories.Identity;
using Web.Persistence.Contexts;
using Web.Persistence.Repositories.Finances;
using Web.Persistence.Repositories.Identity;
using Web.Persistence.Services;

namespace Web.Persistence.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddFinanceDbContext(configuration);
            //     services.AddFinanceDbContext(configuration);
            services.AddServices();
        }
        public static void AddFinanceDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("FinanceConnection");

            services.AddDbContext<FinanceDbContext>(options =>
               options.UseSqlServer(connectionString,
                   builder => builder.MigrationsAssembly("WebJob")));
            services.AddTransient<IFinanceUnitOfWork, FinanceUnitOfWork>();
            services
                .AddTransient<IIdentityUnitOfWork, IdentityUnitOfWork>()
                .AddTransient<IUserRepo, UserRepo>()
                .AddTransient<IRoleRepo, RoleRepo>()
                .AddTransient<IUserRoleRepo, UserRoleRepo>()
                .AddTransient<ISysFunctionRepo, SysFunctionRepo>();
        }
        //public static void AddFinanceDbContext(this IServiceCollection services, IConfiguration configuration)
        //{
        //    var connectionString = configuration.GetConnectionString("FinanceConnection");

        //    services.AddDbContext<FinanceDbContext>(options =>
        //       options.UseSqlServer(connectionString,
        //           builder =>
        //           {
        //               builder.MigrationsAssembly("WebJob");
        //           }));

        //    services.AddTransient<IFinanceUnitOfWork, FinanceUnitOfWork>();
        //}

        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IAuditableService, AuditableServicev2>();
            //    services.AddTransient<IGenericDbService, GenericDbService>();
        }
    }
}
