using IC.WebJob.Helpers.Configs;
using Microsoft.Identity.Client;
using System.Security.Claims;

namespace IC.WebJob.Helpers
{
    public static class HttpContextHelper
    {
        public static int GetUserId(HttpContext context, bool checkMapUserId = true)
        {
            if (context == null) return 0;

            int userId = 0;
            Claim claim;

            if (checkMapUserId)
            {
                claim = context.User.Claims.FirstOrDefault(o => o.Type == KeyConfig.MapUserIdKey);
                if (claim != null) _ = int.TryParse(claim.Value, out userId);
                if (userId > 0) return userId;
            }

            claim = context.User.Claims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier);
            if (claim == null) return 0;

            _ = int.TryParse(claim.Value, out userId);

            return userId;
        }
        public static List<int> GetUserRoleId(HttpContext context)
        {
            int userId = GetUserId(context);
            if (userId > 0)
            {
                var roleIds = context.Session.GetString(KeyConfig.UserRoleKey);
                if (!string.IsNullOrEmpty(roleIds))
                {
                    return roleIds.Split(',').ToList().Where(x => int.TryParse(x, out _)).Select(x => int.Parse(x)).ToList();
                }
            }
            return null;
        }

        public static string GetMenuCollapseExpand(HttpContext context)
        {
            if (context == null) return "";

            var claim = context.User.Claims.FirstOrDefault(o => o.Type == KeyConfig.MenuCollapseExpandKey);
            if (claim == null) return "";

            var menuState = claim.Value;
            if (!string.IsNullOrEmpty(context.Session.GetString(KeyConfig.MenuCollapseExpandKey)))
            {
                menuState = context.Session.GetString(KeyConfig.MenuCollapseExpandKey);
            }

            return menuState;
        }

        public static string GetClaimValue(HttpContext context, string claimType)
        {
            if (context == null) return "";

            var claim = context.User.Claims.FirstOrDefault(o => o.Type == claimType);
            if (claim == null) return "";

            return claim.Value;
        }

        public static bool IsInWhiteListIp(HttpContext context)
        {
            if (!string.IsNullOrEmpty(AppConfig.AppSettings.IpWhiteList))
            {
                if (!AppConfig.AppSettings.IpWhiteList.Contains(context.Request.Host.Host)) return false;
            }

            return true;
        }

    }

}
