using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using WebJob.Helpers.Configs;
using MediatR;
using Web.Application.Features.IdentityFeatures.Users.Queries;

namespace WebJob.Helpers.Security
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class CmsAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        void IAuthorizationFilter.OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (user.Identity == null) return;

            if (!user.Identity.IsAuthenticated)
            {
                // it isn't needed to set unauthorized result 
                // as the base class already requires the user to be authenticated
                // this also makes redirect to a login page work properly
                // context.Result = new UnauthorizedResult();
                return;
            }
            var userId = HttpContextHelper.GetUserId(context.HttpContext, false);

            //Check change pass
            //if (AppConfig.AppSettings.RequireChangePassFirstTime && !bool.Parse(HttpContextHelper.GetClaimValue(context.HttpContext, KeyConfig.ChangePassFirstTimeKey)))
            //{
            //    context.Result = new RedirectResult(AppConfig.LinkChangePass);
            //    return;
            //}

            //Check authen 2FA
            if (AppConfig.AppSettings.RequireAuth2Fa)
            {
                IMediator mediator = context.HttpContext.RequestServices.GetService(typeof(IMediator)) as IMediator;
                if (!PermissionHelper.UserGetTwoFactorEnabled(mediator, context.HttpContext))
                {
                    context.Result = new RedirectResult(AppConfig.LinkSetAuthen2FA);
                    return;
                }
            }

            // you can also use registered services
            var userService = context.HttpContext.RequestServices.GetService(typeof(IAuthorizeService)) as IAuthorizeService;
            if (userService == null) return;

            bool isAuthorized;
            if (context.HttpContext.Session.TryGetValue(KeyConfig.ListSysFunctionsCheckPermissionKey, out var sessionData))
            {
                isAuthorized = userService.HasPermission(userId, context.HttpContext.Request.Path);
                //isAuthorized = userService.HasPermission(context.HttpContext.Request.Path, sessionData);
            }
            else
            {
                //isAuthorized = userService.HasPermission(user.Identity.Name, context.HttpContext.Request.Path);
                isAuthorized = userService.HasPermission(userId, context.HttpContext.Request.Path);
            }
            if (!isAuthorized)
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}
