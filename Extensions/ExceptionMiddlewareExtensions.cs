using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace BatchAPI.Extensions
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ExceptionMiddlewareExtensions
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddlewareExtensions(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context) => this.InvokeAsync(context);

        async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            string result;

            var errorMessage = new
            {
                correaltionId = Guid.NewGuid(),
                    error = exception.Message,
                    stack = exception.StackTrace,
                    innerException = exception.InnerException
                    
                };

                result = JsonConvert.SerializeObject(errorMessage);

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(result);
        }

    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionMiddlewareExtensionsExtensions
    {
        public static IApplicationBuilder UseExceptionMiddlewareExtensions(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddlewareExtensions>();
        }
    }

}
