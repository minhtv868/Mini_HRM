namespace WebAPI.Utils
{
    public static class RequestUtil
    {
        public static string GetIpAddress(HttpContext context)
        {
            string ipAddress = context.Request.Headers["X-Forwarded-For"];

            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = context.Connection.RemoteIpAddress.ToString();
            }

            return ipAddress;
        }

        public static string GetUserAgent(HttpContext context)
        {
            return context.Request.Headers.UserAgent;
        }

        public static string GetReferer(HttpContext context)
        {
            return context.Request.Headers.Referer.ToString();
        }
    }
}
