using IC.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace IC.Infrastructure.Services
{
	public class AppClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, Role>
	{
		private readonly RoleManager<Role> _roleManager;
		private readonly UserManager<User> _userManager;
		public AppClaimsPrincipalFactory(UserManager<User> userManager,
			RoleManager<Role> roleManager,
			IOptions<IdentityOptions> optionsAccessor) : base(userManager, roleManager, optionsAccessor)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}

		public override async Task<ClaimsPrincipal> CreateAsync(User user)
		{
			var principal = await base.CreateAsync(user);

			if (!string.IsNullOrEmpty(user.FullName))
			{
				((ClaimsIdentity)principal.Identity).AddClaims(new[] {
					new Claim(ClaimTypes.GivenName, user.FullName)
				});
			}

			if (!string.IsNullOrEmpty(user.AvatarPath))
			{
				((ClaimsIdentity)principal.Identity).AddClaims(new[] {
					new Claim("AvatarPath", user.AvatarPath)
				});
			}

			var appUser = await _userManager.FindByIdAsync(user.Id.ToString());

			var roles = await _userManager.GetRolesAsync(appUser);

			foreach (var rolename in roles)
			{
				var role = await _roleManager.FindByNameAsync(rolename);
				var claims = await _roleManager.GetClaimsAsync(role);
				foreach (var claim in claims)
				{
					((ClaimsIdentity)principal.Identity).AddClaim(claim);
				}
				((ClaimsIdentity)principal.Identity).AddClaims(
					new[]
					{
						new Claim(ClaimTypes.Role, rolename)
					}
				);
			}

			return principal;
		}
	}
}
