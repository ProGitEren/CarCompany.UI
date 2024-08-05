using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Infrastructure
{
    public static class LoggingRequistration
    {
        public static void SeriLogConfiguration(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((context, services, configuration) => configuration
          .ReadFrom.Configuration(context.Configuration)
          .ReadFrom.Services(services)
          .Enrich.FromLogContext()
          .Enrich.WithProperty("ApplicationName", context.HostingEnvironment.ApplicationName)
          .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
          .WriteTo.Console()
          .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day));

        }
    }

}
