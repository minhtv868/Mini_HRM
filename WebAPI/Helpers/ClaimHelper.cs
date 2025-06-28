using System.Security.Claims;

namespace WebAPI.Helpers
{
    public static class ClaimHelper
    {
        public static int GetCustomerId(this ClaimsPrincipal user)
        {
            _ = int.TryParse(GetClaimValue(user, ConstantHelper.ClaimTypeCustomerId), out var id);

            return id;
        }

        public static short GetSiteId(this ClaimsPrincipal user)
        {
            _ = short.TryParse(GetClaimValue(user, ConstantHelper.ClaimTypeSiteId), out var id);

            return id;
        }

        public static string GetCustomerName(this ClaimsPrincipal user)
        {
            string customerName = GetClaimValue(user, ConstantHelper.ClaimTypeCustomerName);

            if (!string.IsNullOrWhiteSpace(customerName))
            {
                int atIndex = customerName.IndexOf('@');

                if (atIndex > 0)
                {
                    return customerName.Substring(0, atIndex);
                }
            }

            return customerName;
        }

        public static string GetCustomerMail(this ClaimsPrincipal user)
        {
            return GetClaimValue(user, ConstantHelper.ClaimTypeCustomerMail);
        }

        public static string GetCustomerMobile(this ClaimsPrincipal user)
        {
            return GetClaimValue(user, ConstantHelper.ClaimTypeCustomerMobile);
        }

        public static string GetCustomerFullName(this ClaimsPrincipal user)
        {
            return GetClaimValue(user, ConstantHelper.ClaimTypeCustomerFullName);
        }

        public static string GetCustomerAvatarPath(this ClaimsPrincipal user)
        {
            string avatarPath = GetClaimValue(user, ConstantHelper.ClaimTypeCustomerAvatar);

            if (string.IsNullOrWhiteSpace(avatarPath))
            {
                return "/images/icon-15.png";
            }

            return avatarPath;
        }

        public static byte GetCustomerLockPassword(this ClaimsPrincipal user)
        {
            _ = byte.TryParse(GetClaimValue(user, ConstantHelper.ClaimTypeCustomerLockPassword), out var id);

            return id;
        }

        public static short GetCustomerGroupId(this ClaimsPrincipal user)
        {
            _ = short.TryParse(GetClaimValue(user, ConstantHelper.ClaimTypeCustomerGroupId), out var id);

            return id;
        }

        public static string GetDataRoleIds(this ClaimsPrincipal user)
        {
            return GetClaimValue(user, ConstantHelper.ClaimTypeCustomerDataRoleIds);
        }

        public static string GetSiteShortName(this ClaimsPrincipal user)
        {
            return GetClaimValue(user, ConstantHelper.ClaimTypeSiteShortName);
        }

        public static string GetClaimValue(this ClaimsPrincipal user, string claimType)
        {
            if (user.Identity.IsAuthenticated && user.Claims.Any())
            {
                var claim = user.Claims.FirstOrDefault(c => c.Type == claimType);

                if (claim != null)
                {
                    return claim.Value;
                }
            }

            return string.Empty;
        }
    }
}
