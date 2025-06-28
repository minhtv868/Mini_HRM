namespace WebAPI.Filters
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("handling the remote login"))
                {
                    _logger.LogError(ex, "An unhandled exception has occurred.");
                    httpContext.Response.Redirect("/Account/AccessDenied");
                    return;
                }
                _logger.LogError(ex, "An unhandled exception has occurred.");

                throw;
            }
        }
    }

}
