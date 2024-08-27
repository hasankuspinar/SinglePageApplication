using System.Net;
namespace SPAproj.AccountService.Repo
{
    public class IpRestrictionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<IpRestrictionMiddleware> _logger;

        public IpRestrictionMiddleware(RequestDelegate next, ILogger<IpRestrictionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var remoteIp = context.Connection.RemoteIpAddress;

            _logger.LogInformation("Request from Remote IP address: {RemoteIp}", remoteIp);

            
            if (remoteIp.Equals(IPAddress.Loopback) || remoteIp.Equals(IPAddress.IPv6Loopback))
            {
                await _next(context);
            }
            else
            {
                _logger.LogWarning("Forbidden Request from IP: {RemoteIp}", remoteIp);
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.WriteAsync("Access denied");
            }
        }
    }


}
