using Infrastructure.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Text.Json;

namespace Infrastructure.MiddleWare
{
    public class ExceptionMiddleware
    {
        //Asp.Net Core 8 Web API :https://www.youtube.com/watch?v=UqegTYn2aKE&list=PLazvcyckcBwitbcbYveMdXlw8mqoBDbTT&index=1

        private readonly RequestDelegate _next;
        private readonly Serilog.ILogger _logger;
        private readonly IHostEnvironment _hostEnvironment;

        public ExceptionMiddleware(RequestDelegate next, Serilog.ILogger logger, IHostEnvironment hostEnvironment)
        {
            _next = next;
            _logger = logger;
            _hostEnvironment = hostEnvironment;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
                _logger.Information("Success, No Problem in Middleware");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, $"This Error come From exception Middleware {ex.Message} !");


                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";


                // Checks wheter in development environment or not according to that changes the response format

                var response = _hostEnvironment.IsDevelopment()
                    ? new ApiException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
                    : new ApiException((int)HttpStatusCode.InternalServerError);


                // Json format conversion from object

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var json = JsonSerializer.Serialize(response, options);
                await context.Response.WriteAsync(json);

            }
        }
    }
}
