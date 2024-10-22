using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using NuGet.Common;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Middleware
{
    public class CookieRenewalMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookieRenewalMiddleware(RequestDelegate next, IHttpContextAccessor httpContextAccessor)
        {
            _next = next;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var cookie = context.Request.Cookies["JwtToken"];
           
            if (!string.IsNullOrEmpty(cookie))
            {
                var cookieValue = JsonConvert.DeserializeObject<CookieModel>(cookie);
                if (cookieValue is not null && cookieValue.Expiration <= DateTime.UtcNow)
                {
                    RenewCookie(cookieValue, context);
                }
            }

            await _next(context);
        }

        private void RenewCookie(CookieModel cookieValue, HttpContext context)
        {
            //As it will not be expired at this time we need to delete the old cookie
            context.Response.Cookies.Delete("JwtToken", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(30),
            });

            var newCookieValue = JsonConvert.SerializeObject(new CookieModel { Token = cookieValue.Token, Expiration = DateTime.UtcNow.AddMinutes(30) });

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(30),
            };


            context.Response.Cookies.Append("JwtToken", newCookieValue, cookieOptions);
        }
    }
}
