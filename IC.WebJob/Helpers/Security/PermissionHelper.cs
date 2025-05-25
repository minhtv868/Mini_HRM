using IC.Application.Features.IdentityFeatures.SysFunctions.Queries;
using IC.Application.Features.IdentityFeatures.Users.Queries;
using IC.Domain.Entities.Identity;
using IC.WebJob.Helpers.Configs;
using MediatR;

namespace IC.WebJob.Helpers.Security
{
    public static class PermissionHelper
    {
        public static bool UserHasPermissionOnUrl(IMediator mediator, HttpContext context, string url)
        {
            if (context == null || mediator == null) return false;

            int userId = HttpContextHelper.GetUserId(context);
            var result = mediator.Send(new SysFunctionCheckPermissionByUserQuery(userId, url)).GetAwaiter().GetResult();
            return result.Data;
        }

        public static bool UserGetTwoFactorEnabled(IMediator mediator, HttpContext context)
        {
            if (context == null || mediator == null) return false;

            bool twoFactorEnabled;
            var userId = HttpContextHelper.GetUserId(context, false);
            var data = context.Session.GetString(KeyConfig.TwoFactorEnabledKey + userId);
            if (data != null)
            {
                twoFactorEnabled = bool.Parse(data);
            }
            else
            {
                var result = mediator.Send(new UserGetByIdQuery() { Id = userId }).GetAwaiter().GetResult();
                twoFactorEnabled = result.Data.TwoFactorEnabled;
                context.Session.SetString(KeyConfig.TwoFactorEnabledKey + userId, twoFactorEnabled.ToString());
            }
            
            return twoFactorEnabled;
        }
    }
}
