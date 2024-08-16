namespace SPAproj.Server.Repo
{
    public class AdminRoleMiddleware
    {
        private readonly RequestDelegate _next;

        public AdminRoleMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated && context.User.IsInRole("admin"))
            {
                await _next(context);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Access is denied.");
            }
        }
    }

}
