using IC.Application.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace IC.WebJob.Filters
{
    public class ApiKeyFilter : Attribute, IAuthorizationFilter
    {
        private const string APIKEY = "x-api-key";
        private readonly string _requiredApiKey;

        public ApiKeyFilter(IOptions<AppSettings> appSettings)
        {
            _requiredApiKey = appSettings.Value.PostDataApiKey;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var endpoint = context.ActionDescriptor.EndpointMetadata;
            if (endpoint.Any(metadata => metadata is AllowAnonymousAttribute))
            {
                return;
            }
            var headers = context.HttpContext.Request.Headers;
            if (!headers.TryGetValue(APIKEY, out var extractedApiKey))
            {
                context.Result = new UnauthorizedObjectResult("API Key was not provided.");
                return;
            }

            if (extractedApiKey != _requiredApiKey)
            {
                context.Result = new UnauthorizedObjectResult("Unauthorized client.");
                return;
            }
        }
    }
}
