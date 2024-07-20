using Azure.Core.Serialization;
using Ecom.API.Errors;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Text.Json;

namespace Ecom.API.MiddleWare
{
    public class ExceptionMiddleWare
    {
        private readonly IHostEnvironment _hostEnvironment;

        public ExceptionMiddleWare(RequestDelegate next, ILogger<ExceptionMiddleWare> logger,IHostEnvironment hostEnvironment)
        {
            _next = next;
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }

        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleWare> _logger;

        public async Task InvokeAsync(HttpContext context)
        {

            try
            {
                await _next(context);
                _logger.LogInformation("Success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,$"This Error Come From Excption MiddleWare{ex.Message} !");

                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var response = _hostEnvironment.IsDevelopment()?
                   new ApiException(context.Response.StatusCode,ex.Message,ex.StackTrace.ToString())
                   : new ApiException(context.Response.StatusCode)
                   ;
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response,options);
                await context.Response.WriteAsync(json);
               

               
            }
        }
    }
}
