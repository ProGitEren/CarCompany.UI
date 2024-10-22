using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Handlers
{
    public class AuthenticationHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public AuthenticationHandler(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken = default)
        {
            var cookieValue = _contextAccessor.HttpContext?.Request.Cookies["JwtToken"];

            if (!string.IsNullOrEmpty(cookieValue))
            {
                // Deserialize the cookie to extract the token
                var cookieData = JsonConvert.DeserializeObject<CookieModel>(cookieValue);

                if (cookieData != null && !string.IsNullOrEmpty(cookieData.Token))
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", cookieData.Token);
                }
            }

            return base.SendAsync(request, cancellationToken);
        }
            
            
    }
}
