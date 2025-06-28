using System.Text.RegularExpressions;

namespace WebAPI.Filters
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string APIKEY_HEADER = "x-api-key";
        private const string VALID_API_KEY = "1234"; // key cố định

        private static readonly Regex _bypassRegex = new(
            @"(AIdocs|health)",
            RegexOptions.IgnoreCase | RegexOptions.Compiled
        );

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (_bypassRegex.IsMatch(context.Request.Path))
            {
                await _next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue(APIKEY_HEADER, out var extractedApiKey))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("API Key was not provided.");
                return;
            }

            if (extractedApiKey != VALID_API_KEY)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized client.");
                return;
            }

            await _next(context);
        }
    }
}
