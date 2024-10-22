using Infrastructure.Handlers;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configuration
{
    public static class CarCompanyConfiguration
    {
        private static void ConfigureDefaultHttpClient(IServiceProvider sp, HttpClient client)
        {
            var defaultClient = sp.GetRequiredService<IHttpClientFactory>()
                                  .CreateClient("DefaultClient");

            client.BaseAddress = defaultClient.BaseAddress;
            client.Timeout = defaultClient.Timeout;
            //foreach (var header in defaultClient.DefaultRequestHeaders)
            //{
            //    client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
            //}
        }

        private static HttpMessageHandler ConfigureMessageHandler()
        {
            return new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(5)
            };
        }

        public static IServiceCollection ApiServiceConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddTransient<RetryHandler>();
            services.AddTransient<LoggingHttpMessageHandler>();
            services.AddTransient<AuthenticationHandler>();

            services.AddHttpClient("DefaultClient").ConfigureHttpClient((client) =>
            {
                client.BaseAddress = new Uri("https://localhost:7218");
                client.Timeout = TimeSpan.FromSeconds(30);

                //var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                //var cookieValue = httpContextAccessor.HttpContext?.Request.Cookies["JwtToken"];

                //if (!string.IsNullOrEmpty(cookieValue))
                //{
                //    // Deserialize the cookie to extract the token
                //    var cookieData = JsonConvert.DeserializeObject<CookieModel>(cookieValue);

                //    if (cookieData != null && !string.IsNullOrEmpty(cookieData.Token))
                //    {
                //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", cookieData.Token);
                //    }
                //}
            });
            //.AddHttpMessageHandler<LoggingHttpMessageHandler>()
            //.AddHttpMessageHandler<RetryHandler>(); 


            services.AddHttpClient<IUserService, UserService>()
                    .ConfigureHttpClient(ConfigureDefaultHttpClient)
                    .ConfigurePrimaryHttpMessageHandler(ConfigureMessageHandler)
                    .SetHandlerLifetime(TimeSpan.FromDays(1))
                    .AddHttpMessageHandler<RetryHandler>()
                    .AddHttpMessageHandler<LoggingHttpMessageHandler>()
                    .AddHttpMessageHandler<AuthenticationHandler>();

            services.AddHttpClient<IVehicleService, VehicleService>()
                    .ConfigureHttpClient(ConfigureDefaultHttpClient)
                    .ConfigurePrimaryHttpMessageHandler(ConfigureMessageHandler)
                    .SetHandlerLifetime(TimeSpan.FromDays(1))
                    .AddHttpMessageHandler<RetryHandler>()
                    .AddHttpMessageHandler<LoggingHttpMessageHandler>()
                    .AddHttpMessageHandler<AuthenticationHandler>();

            services.AddHttpClient<IOrderVehicleService, OrderVehicleService>()
                    .ConfigureHttpClient(ConfigureDefaultHttpClient)
                    .ConfigurePrimaryHttpMessageHandler(ConfigureMessageHandler)
                    .SetHandlerLifetime(TimeSpan.FromDays(1))
                    .AddHttpMessageHandler<RetryHandler>()
                    .AddHttpMessageHandler<LoggingHttpMessageHandler>()
                    .AddHttpMessageHandler<AuthenticationHandler>();


            services.AddHttpClient<IVehicleModelService, VehicleModelService>()
                    .ConfigureHttpClient(ConfigureDefaultHttpClient)
                    .ConfigurePrimaryHttpMessageHandler(ConfigureMessageHandler)
                    .SetHandlerLifetime(TimeSpan.FromDays(1))
                    .AddHttpMessageHandler<RetryHandler>()
                    .AddHttpMessageHandler<LoggingHttpMessageHandler>()
                    .AddHttpMessageHandler<AuthenticationHandler>();


            services.AddHttpClient<IEngineService, EngineService>()
                    .ConfigureHttpClient(ConfigureDefaultHttpClient)
                    .ConfigurePrimaryHttpMessageHandler(ConfigureMessageHandler)
                    .SetHandlerLifetime(TimeSpan.FromDays(1))
                    .AddHttpMessageHandler<RetryHandler>()
                    .AddHttpMessageHandler<LoggingHttpMessageHandler>()
                    .AddHttpMessageHandler<AuthenticationHandler>();


            services.AddHttpClient<IOrderService, OrderService>()
                    .ConfigureHttpClient(ConfigureDefaultHttpClient)
                    .ConfigurePrimaryHttpMessageHandler(ConfigureMessageHandler)
                    .SetHandlerLifetime(TimeSpan.FromDays(1))
                    .AddHttpMessageHandler<RetryHandler>()
                    .AddHttpMessageHandler<LoggingHttpMessageHandler>()
                    .AddHttpMessageHandler<AuthenticationHandler>();

            services.AddHostedService<CookieRefreshService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.AccessDeniedPath = "/Account/Profile";
                }
                );

            services.AddAuthorization(options =>
            {
                // Alternative way
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
                options.AddPolicy("Buyer", policy => policy.RequireRole("Buyer"));
                options.AddPolicy("Seller", policy => policy.RequireRole("Seller"));
            });

            services.AddScoped<EncryptionService>();

            return services;
        }

    }
}
