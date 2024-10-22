using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Mappings
{
    public static class EngineMapper
    {
        public static  Dictionary<string, string> EngineMapping = new Dictionary<string, string>
                {
                    { "EN123", "Standard 2.0L Inline 4 Engine" },
                    { "EN456", "High Output 3.5L V6 Engine" },
                    { "EN789", "Performance 4.0L V8 Engine" },
                    { "EN101", "Economy 1.6L Inline 4 Engine" },
                    { "EN202", "Economy 2.0L Inline 4 Engine" },

                    { "F01V8", "Ford V8 Engine" },
                    { "F02EM", "Ford Electric Motor" },
                    { "F03HY", "Ford Hybrid Engine" },
                    { "F04V6", "Ford V6 Turbo Engine" },

                    { "T01V8", "Toyota V8 Engine" },
                    { "T02EM", "Toyota Electric Motor" },
                    { "T03HY", "Toyota Hybrid Engine" },
                    { "T04V6", "Toyota V6 Turbo Engine" },
                    
                    { "B01V8", "BMW V8 Engine" },
                    { "B02EM", "BMW Electric Motor" },
                    { "B03HY", "BMW Hybrid Engine" },
                    { "B04V6", "BMW V6 Turbo Engine" },

                    { "H01V8", "Honda V8 Engine" },
                    { "H02EM", "Honda Electric Motor" },
                    { "H03HY", "Honda Hybrid Engine" },
                    { "H04V6", "Honda V6 Turbo Engine" },

                    { "M01V8", "Mercedes V8 Engine" },
                    { "M02EM", "Mercedes Electric Motor" },
                    { "M03HY", "Mercedes Hybrid Engine" },
                    { "M04V6", "Mercedes V6 Turbo Engine" }
                };

    
    }
}
