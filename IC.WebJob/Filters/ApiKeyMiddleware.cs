
using IC.Application.Settings;
using MediatR;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace IC.WebJob.Filters
{
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string APPNAME = "bongda24hcms";
        private const string APIKEY = "x-api-key";
        private readonly IOptions<AppSettings> _appSettings;

        public ApiKeyMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //Bỏ qua một số request ko check ApiKey 
            //if (Regex.IsMatch(context.Request.Path, "", RegexOptions.IgnoreCase))
            //{
            //    await _next(context);
            //    return;
            //}

            if (!context.Request.Headers.TryGetValue(APIKEY, out var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("API Key was not provided.");
                return;
            }

            if (extractedApiKey != _appSettings.Value.PostDataApiKey)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized client.");
                return;
            }
            await _next(context);
        }
    }
}
