using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace SPAproj.Server.Repo
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is UnauthorizedAccessException)
            {
                return;
            }
            var apiError = new ApiError
            {
                Message = "An error occurred while processing your request.", 
                Detail = context.Exception.ToString()
            };

            context.Result = new ObjectResult(apiError)
            {
                StatusCode = 500 
            };

            context.ExceptionHandled = true;
        }

        public class ApiError
        {
            public string Message { get; set; }
            public string Detail { get; set; }
        }
    }
}
