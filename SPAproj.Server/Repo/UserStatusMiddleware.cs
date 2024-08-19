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
            if (context.Request.Path.StartsWithSegments("/api/auth/logout"))
            {
                await _next(context);
                return;
            }

            var userStatusClaim = context.User.Claims.FirstOrDefault(c => c.Type == "UserStatus");

            if (userStatusClaim != null && userStatusClaim.Value == "0") // 0 is blocked, 1 is ok
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Access denied - user is blocked.");
                return;
            }

            await _next(context);
        }
    }
}
