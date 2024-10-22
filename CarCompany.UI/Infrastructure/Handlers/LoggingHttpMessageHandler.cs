using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Handlers
{
    public class LoggingHttpMessageHandler : DelegatingHandler
    {
        private readonly Serilog.ILogger _logger;

        public LoggingHttpMessageHandler(Serilog.ILogger logger) 
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
           
            try 
            {
                // Log request details
                _logger.Information("Sending HTTP request: {Method} {RequestUri} Headers: {Headers}",
                    request.Method, request.RequestUri, request.Headers);

                // Send the request to the next handler
                var response = await base.SendAsync(request, cancellationToken);

                // Log response details
                _logger.Information("Received HTTP response: {StatusCode} for {RequestUri} Headers: {Headers}",
                    response.StatusCode, request.RequestUri, response.Headers);

                return response;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "HTTP Request failed.");

                throw;
            }
        }
    }
}
