using Microsoft.AspNetCore.Authorization;

public enum Status
{
    Blocked = 0,
    Active = 1
}

namespace SPAproj.Server.Repo
{
    public class UserStatusMiddleware
    {
        private readonly RequestDelegate _next;

        public UserStatusMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            if (endpoint != null)
            {
                if (endpoint.Metadata.GetMetadata<AllowAnonymousAttribute>() != null)
                {
                    await _next(context);
                    return;
                }
            }
            if (!context.User.Identity.IsAuthenticated)
            {
                await _next(context);
                return;
            }

            var userStatusClaim = context.User.Claims.FirstOrDefault(c => c.Type == "UserStatus");

            if (userStatusClaim != null && Enum.TryParse(userStatusClaim.Value, out Status status) && status == Status.Blocked) 
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Access denied - user is blocked.");
                return;
            }

            await _next(context);
        }
    }
}
