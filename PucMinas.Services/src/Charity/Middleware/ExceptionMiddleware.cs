using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PucMinas.Services.Charity.Domain.Results.Exceptions;
using System;
using System.Net;
using System.Threading.Tasks;

namespace PucMinas.Services.Charity.Middleware
{
    public class ExceptionMiddleware
    {
        public RequestDelegate Next { get; }

        public ExceptionMiddleware(RequestDelegate next)
        {
            Next = next;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await Next.Invoke(context);
            }
            catch (Exception ex)
            {
                // Logar a Exception em arquivo/splunk.
                ErrorMessage error = new ErrorMessage((int)HttpStatusCode.InternalServerError, "An error occur. Please try again or contact the system administrator.");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                await context.Response.WriteAsync(JsonConvert.SerializeObject(error));
            }
        }
    }
}
