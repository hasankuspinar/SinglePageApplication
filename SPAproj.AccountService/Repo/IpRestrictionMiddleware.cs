using SPAproj.Models.Service;
using System.Net;
namespace SPAproj.AccountService.Repo
{
    public class IpRestrictionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<IpRestrictionMiddleware> _logger;
        private readonly ConfigurationService _configuration;

        public IpRestrictionMiddleware(RequestDelegate next, ILogger<IpRestrictionMiddleware> logger, ConfigurationService configuration)
        {
            _next = next;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            var remoteIp = context.Connection.RemoteIpAddress;

            _logger.LogInformation("Request from Remote IP address: {RemoteIp}", remoteIp);


            var allowedIp = _configuration.GetParameterValue("serverapi");

            if (IPAddress.TryParse(allowedIp, out var allowedIPAddress) && remoteIp.Equals(allowedIPAddress))
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
