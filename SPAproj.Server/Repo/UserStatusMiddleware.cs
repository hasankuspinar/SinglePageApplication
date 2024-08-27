using Microsoft.AspNetCore.Authorization;
using SPAproj.Server.Models;

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

            if (endpoint != null && endpoint.Metadata.GetMetadata<AllowAnonymousAttribute>() != null)
            {
                await _next(context);
                return;
            }
            else if (context.User.Identity.IsAuthenticated)
            {
                var username = context.User.Identity.Name;
                if (!string.IsNullOrEmpty(username))
                {
                    var userRepository = context.RequestServices.GetService<IUserRepository>();
                    var user = await userRepository.GetUserByUsername(username);
                    if (user != null)
                    {
                        var userStatus = await userRepository.GetUserStatus(user.UserId);

                        if (userStatus != null && userStatus.Status == (int)Status.Blocked)
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            await context.Response.WriteAsync("Access denied - user is blocked.");
                            return;
                        }
                    }
                }
            }
            await _next(context);
        }
    }
}
