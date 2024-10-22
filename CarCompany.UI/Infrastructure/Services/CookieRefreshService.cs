using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

public class CookieRefreshService : BackgroundService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly TimeSpan _checkInterval = TimeSpan.FromSeconds(30);

    public CookieRefreshService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            CheckAndRenewCookie();
            await Task.Delay(_checkInterval, stoppingToken);
        }
    }

    private void CheckAndRenewCookie()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context == null) return;

        var cookie = context.Request.Cookies["JwtToken"];
        if (string.IsNullOrEmpty(cookie)) return;

        // Assuming expiration time is stored within the cookie value as a JSON object, or in a separate cookie.
        var cookieData = JsonConvert.DeserializeObject<CookieModel>(cookie);// Implement this method based on your storage strategy

        if (cookieData is not null && cookieData.Expiration <= DateTime.UtcNow.AddMinutes(1))
        {
            // Renew the cookie
            RenewCookie(cookieData.Token);
        }
    }
        
   

    private void RenewCookie(string token) 
    {
        var context = _httpContextAccessor.HttpContext;

        //As it will not be expired at this time we need to delete the old cookie
        context.Response.Cookies.Delete("JwtToken", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddMinutes(30),
        });

        var newCookieValue = JsonConvert.SerializeObject(new CookieModel { Token = token, Expiration = DateTime.UtcNow.AddMinutes(30) });

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