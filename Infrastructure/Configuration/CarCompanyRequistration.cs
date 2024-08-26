using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace Infrastructure
{
    public static class CarCompanyRequistration
    {

        public static IServiceCollection ApiServiceConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            //var servicesToRegister = new List<HttpClient>
            //{
            //    UserService,
            //    typeof(VehicleService),
            //    typeof(OrderVehicleService),
            //    typeof(VehicleModelService),
            //    typeof(EngineService),
            //    typeof(OrderService)
            //};

            //foreach (var serviceType in servicesToRegister )
            //{
            //    services.AddHttpClient<ApiService>(client =>
            //    {
            //        client.BaseAddress = new Uri("https://localhost:7218");
            //        client.Timeout = TimeSpan.FromSeconds(30);
            //    });
            //}


            services.AddHttpClient<IUserService,UserService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7218");
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            services.AddHttpClient<VehicleService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7218");
                client.Timeout = TimeSpan.FromSeconds(30);

            });
            services.AddHttpClient<OrderVehicleService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7218");
                client.Timeout = TimeSpan.FromSeconds(30);

            });

            services.AddHttpClient<VehicleModelService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7218");
                client.Timeout = TimeSpan.FromSeconds(30);

            });

            services.AddHttpClient<EngineService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7218");
                client.Timeout = TimeSpan.FromSeconds(30);

            });

            services.AddHttpClient<OrderService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7218");
                client.Timeout = TimeSpan.FromSeconds(30);

            });

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
