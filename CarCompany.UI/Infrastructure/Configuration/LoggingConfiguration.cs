using Microsoft.AspNetCore.Builder;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configuration
{
    public static class LoggingConfiguration
    {
        public static void SeriLogConfiguration(this WebApplicationBuilder builder)
        {
            
            builder.Host.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration));

        }
    }
}
