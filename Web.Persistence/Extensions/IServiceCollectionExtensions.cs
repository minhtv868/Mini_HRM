using Web.Application.Interfaces;
using Web.Application.Interfaces.Repositories;
using Web.Application.Interfaces.Repositories.BongDa24hCrawls;
using Web.Application.Interfaces.Repositories.BongDa24hJobs;
using Web.Application.Interfaces.Repositories.Identity;
using Web.Persistence.Contexts;
using Web.Persistence.Repositories;
using Web.Persistence.Repositories.BongDa24hCrawls;
using Web.Persistence.Repositories.BongDa24hJobs;
using Web.Persistence.Repositories.Identity;
using Web.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Persistence.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityDbContext(configuration);
            services.AddBongDa24hJobDbContext(configuration);
            services.AddBongDa24hCrawlsDbContext(configuration);
            services.AddServices();
        }
        public static void AddIdentityDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("IdentityConnection");

            services.AddDbContext<WebJobDbContext>(options =>
               options.UseSqlServer(connectionString,
                   builder => builder.MigrationsAssembly("WebJob")));

            services
                .AddTransient<IIdentityUnitOfWork, IdentityUnitOfWork>()
                .AddTransient<IUserRepo, UserRepo>()
                .AddTransient<IRoleRepo, RoleRepo>()
                .AddTransient<IUserRoleRepo, UserRoleRepo>()
                .AddTransient<ISysFunctionRepo, SysFunctionRepo>();
        }
        public static void AddBongDa24hJobDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("HangfireConnection");

            services.AddDbContext<BongDa24hJobDbContext>(options =>
               options.UseSqlServer(connectionString,
                   builder =>
                   {
                       builder.MigrationsAssembly("IC.WebApp");
                   }));

            services.AddTransient<IBongDa24hJobUnitOfWork, BongDa24hJobUnitOfWork>();
        }

        public static void AddBongDa24hCrawlsDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("BongDa24hCrawlsConnection");

            services.AddDbContext<BongDa24hCrawlDbContext>(options =>
               options.UseSqlServer(connectionString,
                   builder =>
                   {
                       builder.MigrationsAssembly("IC.WebApp");
                   }));

            services.AddTransient<IBongDa24HCrawlUnitOfWork, BongDa24hCrawlsUnitOfWork>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IAuditableService, AuditableServicev2>();
            services.AddTransient<IGenericDbService, GenericDbService>();
        }
    }
}
