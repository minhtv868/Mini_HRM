using IC.Persistence.Contexts;
using IC.Domain.Entities.Identity;
using IC.WebJob.Helpers.Security;
using IC.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;

namespace IC.WebJob.Areas.Identity.Extensions
{
	public static class IServiceCollectionExtensions
    {
        public static void AddIdentityService(this IServiceCollection services)
        {
            services.AddScoped<AppSignInManager, AppSignInManager>();
            services.AddScoped<IAuthorizeService, AuthorizeService>();

            services.AddDefaultIdentity<User>(
                                x =>
                                {
                                    x.Password.RequireDigit = true;
                                    x.Password.RequireLowercase = true;
                                    x.Password.RequireUppercase = false;
                                    x.Password.RequireNonAlphanumeric = true;
                                    x.Password.RequiredLength = 8;
                                })
                            .AddRoles<Role>()
                            .AddErrorDescriber<LocalizedIdentityErrorDescriber>()
                            .AddEntityFrameworkStores<ICIdentityDbContext>();

			services.AddScoped<IUserClaimsPrincipalFactory<User>, AppClaimsPrincipalFactory>();
		}

    }
}
