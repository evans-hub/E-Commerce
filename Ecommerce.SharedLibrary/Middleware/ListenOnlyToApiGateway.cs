using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecommerce.SharedLibrary.Middleware
{
    public class ListenOnlyToApiGateway
    {
        private readonly RequestDelegate _next;

        public ListenOnlyToApiGateway(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var signedHeader = context.Request.Headers["Api-Gateway"].FirstOrDefault();
            if (string.IsNullOrEmpty(signedHeader))
            {
                int statusCode = StatusCodes.Status503ServiceUnavailable;
                string message = "Sorry, Service Unavailable";
                string title = "Error";

                await ModifyHeader(context, title, statusCode, message);
                return; // Stop further middleware execution
            }

            await _next(context); // Pass the request to the next middleware in the pipeline
        }

        private async Task ModifyHeader(HttpContext context, string title, int statusCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var problemDetails = new ProblemDetails
            {
                Detail = message,
                Title = title,
                Status = statusCode
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
        }
    }
}
