using System.Security.Claims;

namespace IC.Infrastructure.Extensions
{
	public static class ClaimsPrincipalExtensions
	{
		public static int GetUserId(this ClaimsPrincipal claimsPrincipal)
		=> int.Parse(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier));

        public static string GetDisplayName(this ClaimsPrincipal claimsPrincipal)
        => claimsPrincipal.FindFirstValue(ClaimTypes.GivenName);

		public static string GetEmail(this ClaimsPrincipal claimsPrincipal)
		=> claimsPrincipal.FindFirstValue(ClaimTypes.Email);

		public static string GetAvatarPath(this ClaimsPrincipal claimsPrincipal)
			=> claimsPrincipal.FindFirstValue("AvatarPath") ?? "/img/avatar.png";

		public static string GetFullName(this ClaimsPrincipal claimsPrincipal)
		=> claimsPrincipal.FindFirstValue(ClaimTypes.GivenName);

		public static string GetDefaultGroup(this ClaimsPrincipal claimsPrincipal)
		 => claimsPrincipal.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).FirstOrDefault();

		public static string[] GetRoles(this ClaimsPrincipal claimsPrincipal)
		 => claimsPrincipal.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToArray();
	}
}
