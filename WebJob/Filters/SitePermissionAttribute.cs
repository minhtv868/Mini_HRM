using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Web.Application.Interfaces;

namespace WebJob.Filters
{
    public class SitePermissionAttribute : Attribute, IAsyncPageFilter
    {
        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            if (await CheckPermissionsAsync(context))
            {
                await next();
                return;
            }

            context.Result = new ForbidResult();
        }

        public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            return Task.CompletedTask;
        }

        private async Task<bool> CheckPermissionsAsync(PageHandlerExecutingContext context)
        {
            if (!TryGetSiteId(context, out var siteId))
                return true;

            if (siteId <= 0)
                return true;

            var currentUserService = context.HttpContext.RequestServices.GetRequiredService<ICurrentUserService>();

            return await currentUserService.HasPermissionWithSite(siteId);
        }

        private bool TryGetSiteId(PageHandlerExecutingContext context, out int siteId)
        {
            siteId = 0;

            var queryKey = context.HttpContext.Request.Query
                .FirstOrDefault(query => string.Equals(query.Key, "Query.SiteId", StringComparison.OrdinalIgnoreCase));

            // Nếu không tìm thấy "Query.SiteId", tìm theo "siteId"
            if (string.IsNullOrEmpty(queryKey.Key))
            {
                queryKey = context.HttpContext.Request.Query
                    .FirstOrDefault(query => string.Equals(query.Key, "siteId", StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(queryKey.Value) && int.TryParse(queryKey.Value, out var id))
            {
                siteId = id;
                return true;
            }

            if (context.HttpContext.Request.HasFormContentType)
            {
                var formKey = context.HttpContext.Request.Form
                    .FirstOrDefault(form => string.Equals(form.Key, "Command.SiteId", StringComparison.OrdinalIgnoreCase));

                if (!string.IsNullOrEmpty(formKey.Value) && int.TryParse(formKey.Value, out id))
                {
                    siteId = id;
                    return true;
                }
            }

            return false;
        }
    }
}
