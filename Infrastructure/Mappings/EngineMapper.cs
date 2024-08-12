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

                    { "F1V8", "Ford V8 Engine" },
                    { "F2EM", "Ford Electric Motor" },
                    { "F3HY", "Ford Hybrid Engine" },
                    { "F4V6", "Ford V6 Turbo Engine" },

                    { "T1V8", "Toyota V8 Engine" },
                    { "T2EM", "Toyota Electric Motor" },
                    { "T3HY", "Toyota Hybrid Engine" },
                    { "T4V6", "Toyota V6 Turbo Engine" },

                    { "B1V8", "BMW V8 Engine" },
                    { "B2EM", "BMW Electric Motor" },
                    { "B3HY", "BMW Hybrid Engine" },
                    { "B4V6", "BMW V6 Turbo Engine" },

                    { "H1V8", "Honda V8 Engine" },
                    { "H2EM", "Honda Electric Motor" },
                    { "H3HY", "Honda Hybrid Engine" },
                    { "H4V6", "Honda V6 Turbo Engine" },

                    { "M1V8", "Mercedes V8 Engine" },
                    { "M2EM", "Mercedes Electric Motor" },
                    { "M3HY", "Mercedes Hybrid Engine" },
                    { "M4V6", "Mercedes V6 Turbo Engine" }
                };


    }
}
