using Infrastructure.Errors;
using Infrastructure.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Helpers
{
    public static class AuthorizationHelper
    {
        public static void AddAuthorizationHeader(IHttpContextAccessor httpContextAccessor, HttpClient httpClient)
        {
            var token = httpContextAccessor.HttpContext.Request.Cookies["JwtToken"];
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }
}
