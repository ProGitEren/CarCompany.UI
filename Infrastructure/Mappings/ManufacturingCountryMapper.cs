using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Mappings
{
    public static class ManufacturingCountryMapper
    {
        public static readonly Dictionary<int, string> CountryMapping = new Dictionary<int, string>
                {
                    { 1, "USA" },
                    { 2, "Canada" },
                    { 3, "Germany" },
                    { 4, "Japan" },
                    { 5, "France" },
                    { 6, "Italy" },
                    { 7, "United Kingdom" },
                    { 8, "South Korea" },
                    { 9, "China" },
                    { 10, "India" },
                    { 11, "Australia" },
                    { 12, "Mexico" },
                    { 13, "Brazil" },
                    { 14, "Russia" },
                    { 15, "Spain" }
                };

    }
}
